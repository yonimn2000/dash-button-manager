using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using YonatanMankovich.DashButtonCore.EventArguments;
using YonatanMankovich.DashButtonCore.Exceptions;

namespace YonatanMankovich.DashButtonCore
{
    public class DashButtonsNetwork
    {
        /// <summary> The collection of dash buttons. Buttons can be added to this list while the network listener is running. </summary>
        public IList<DashButton> DashButtons { get; } = new List<DashButton>();

        /// <summary> The time during which repeated button presses should be ignored. </summary>
        public int DuplicatePressIgnoreTime { get; set; } = 3000;

        /// <summary> Called when a network listener is started. </summary>
        public event EventHandler<NetworkListenerStartedEventArgs> OnNetworkListenerStarted;

        /// <summary> Called when a dash button is clicked. </summary>
        public event EventHandler<DashButtonClickedEventArgs> OnDashButtonClicked;

        /// <summary> Called when an exception is thrown. </summary>
        public event EventHandler<ExceptionThrownEventArgs> OnExceptionThrown;

        /// <summary> Called when an exception is thrown when performing a button action. </summary>
        public event EventHandler<ActionExceptionThrownEventArgs> OnActionExceptionThrown;

        private ICollection<string> IgnoredDuplicateMacAddresses { get; } = new List<string>();

        /// <summary> Starts listening for dash button presses on all network adapters. </summary>
        public void StartListening()
        {
            try // Try retrieving the device list.
            {
                CaptureDeviceList captureDevices = CaptureDeviceList.Instance;
                if (captureDevices.Count == 0)
                    throw new NoNetworkInterfacesException();

                foreach (ICaptureDevice captureDevice in captureDevices)
                {
                    captureDevice.OnPacketArrival += OnPacketArrival;
                    captureDevice.Open(DeviceMode.Promiscuous, 1000);
                    captureDevice.Filter = "arp"; // Capture only ARP Packets.

                    // Run capture asynchronously.
                    Action action = captureDevice.Capture;
                    action.BeginInvoke(callback => action.EndInvoke(callback), null);

                    OnNetworkListenerStarted?.Invoke(this, new NetworkListenerStartedEventArgs
                    {
                        ListenerDevice = captureDevice.Description,
                        ListenerDeviceMacAddress = captureDevice.MacAddress
                    });
                }
            }
            catch (DllNotFoundException)
            {
                OnExceptionThrown.Invoke(this, new ExceptionThrownEventArgs
                {
                    Exception = new PcapMissingException()
                });
            }
        }

        private async void OnPacketArrival(object sender, CaptureEventArgs captureEventArgs)
        {
            Packet packet = Packet.ParsePacket(captureEventArgs.Packet.LinkLayerType, captureEventArgs.Packet.Data);
            PhysicalAddress packetMac = (packet as EthernetPacket).SourceHardwareAddress;

            // Ignore packets from the capture device and ignore repetitive mac addresses.
            if (!packetMac.Equals(captureEventArgs.Device.MacAddress) && !IgnoredDuplicateMacAddresses.Contains(packetMac.ToString()))
            {
                IgnoredDuplicateMacAddresses.Add(packetMac.ToString());
                _ = Task.Delay(DuplicatePressIgnoreTime).ContinueWith((task) => IgnoredDuplicateMacAddresses.Remove(packetMac.ToString()));

                DashButton clickedDashButton = DashButtons
                    .Where(b => b.MacAddress != null && b.Enabled && b.MacAddress.Equals(packetMac.ToString())).FirstOrDefault();

                if (clickedDashButton != null)
                {
                    OnDashButtonClicked?.Invoke(this, new DashButtonClickedEventArgs
                    {
                        DashButton = clickedDashButton,
                        CaptureDeviceMacAddress = captureEventArgs.Device.MacAddress,
                        CaptureDeviceDescription = captureEventArgs.Device.Description
                    });

                    try
                    {
                        await WebActionHelpers.SendGetRequestAsync(clickedDashButton.ActionUrl);
                    }
                    catch (Exception e)
                    {
                        OnActionExceptionThrown.Invoke(this, new ActionExceptionThrownEventArgs
                        {
                            DashButton = clickedDashButton,
                            Exception = e
                        });
                    }
                }
            }
        }
    }
}