using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using YonatanMankovich.DashButtonCore.EventArguments;
using YonatanMankovich.DashButtonCore.Exceptions;

namespace YonatanMankovich.DashButtonCore
{
    public class NetworkListener
    {
        /// <summary> The time during which repeated MAC address discoveries should be ignored. </summary>
        public int DuplicateMacAddressIgnoreTime { get; set; } = 3000;

        /// <summary> Called when a network listener is started. </summary>
        public event EventHandler<NetworkListenerStartedEventArgs> OnNetworkListenerStarted;

        /// <summary> Called when a MAC address is captured. </summary>
        public event EventHandler<MacAddressCapturedEventArgs> OnMacAddressCaptured;

        /// <summary> Called when an exception is thrown. </summary>
        public event EventHandler<ExceptionThrownEventArgs> OnExceptionThrown;

        private ICollection<string> IgnoredDuplicateMacAddresses { get; } = new List<string>();

        /// <summary> Starts listening for mac addresses on all network adapters. </summary>
        public void Start()
        {
            try // Try retrieving the network device list.
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
                OnExceptionThrown?.Invoke(this, new ExceptionThrownEventArgs
                {
                    Exception = new PcapMissingException()
                });
            }
        }

        private void OnPacketArrival(object sender, CaptureEventArgs captureEventArgs)
        {
            Packet packet = Packet.ParsePacket(captureEventArgs.Packet.LinkLayerType, captureEventArgs.Packet.Data);
            PhysicalAddress packetMacAddress = (packet as EthernetPacket).SourceHardwareAddress;
            string packetMacAddressString = packetMacAddress.ToString();
            ICaptureDevice captureDevice = captureEventArgs.Device;
            string captureDeviceMacAddress = captureDevice.MacAddress.ToString();

            // Ignore packets from the capture device and ignore repetitive mac addresses.
            if (packetMacAddressString.Equals(captureDeviceMacAddress)
                || IgnoredDuplicateMacAddresses.Contains(packetMacAddressString))
                return;

            IgnoredDuplicateMacAddresses.Add(packetMacAddressString);
            _ = Task.Delay(DuplicateMacAddressIgnoreTime).ContinueWith((task)
                => IgnoredDuplicateMacAddresses.Remove(packetMacAddressString));

            OnMacAddressCaptured?.Invoke(this, new MacAddressCapturedEventArgs
            {
                MacAddress = packetMacAddressString,
                CaptureDeviceMacAddress = captureDeviceMacAddress,
                CaptureDeviceDescription = captureDevice.Description
            });
        }
    }
}