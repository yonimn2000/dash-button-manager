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
        public IList<DashButton> DashButtons { get; } = new List<DashButton>();
        public int RepetitiveDiscoveryDelay { get; set; } = 3000;
        public event EventHandler<NetworkListenerStartedEventArgs> OnNetworkListenerStarted;
        public event EventHandler<DashButtonClickedEventArgs> OnDashButtonClicked;
        public event EventHandler<ActionExceptionThrownEventArgs> OnActionExceptionThrown;
        public event EventHandler<ExceptionThrownEventArgs> OnExceptionThrown;

        private IList<string> MacAddressesOnHold { get; } = new List<string>();

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
            if (!packetMac.Equals(captureEventArgs.Device.MacAddress) && !MacAddressesOnHold.Contains(packetMac.ToString()))
            {
                MacAddressesOnHold.Add(packetMac.ToString());
                _ = Task.Delay(RepetitiveDiscoveryDelay).ContinueWith((task) => MacAddressesOnHold.Remove(packetMac.ToString()));

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
                        await WebActionsHelpers.SendGetRequestAsync(clickedDashButton.ActionUrl);
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