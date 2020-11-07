using System;
using System.Linq;
using System.Net.NetworkInformation;
using YonatanMankovich.DashButtonCore.Exceptions;
using YonatanMankovich.DashButtonCore.Models;
using PacketDotNet;
using SharpPcap;

namespace YonatanMankovich.DashButtonCore
{
    public class DashNetwork
    {
        private const int ReadTimeoutMilliseconds = 1000;
        public event EventHandler ListenerStarted;
        public event EventHandler DashButtonProbed;

        public void StartListening()
        {
            try // Try retrieving the device list.
            {
                CaptureDeviceList captureDevices = CaptureDeviceList.Instance;
                if (captureDevices.Count == 0)
                    throw new NoNetworkInterfacesException();

                foreach (ICaptureDevice captureDevice in captureDevices.Where(d => d != null))
                {
                    captureDevice.OnPacketArrival += Device_OnPacketArrival;
                    captureDevice.Open(DeviceMode.Promiscuous, ReadTimeoutMilliseconds);
                    captureDevice.Filter = "arp"; // Capture only ARP Packets.

                    // Run capture asynchronously.
                    Action action = captureDevice.Capture;
                    action.BeginInvoke(callback => action.EndInvoke(callback), null);

                    ListenerStarted?.Invoke(this, new DashListenerResponse
                    {
                        Started = true,
                        Message = $"Started listener on {captureDevice.MacAddress} ({captureDevice.Description})"
                    });
                }
            }
            catch (DllNotFoundException)
            {
                throw new PcapMissingException();
            }
        }

        private void Device_OnPacketArrival(object sender, CaptureEventArgs captureEventArgs)
        {
            Packet packet = Packet.ParsePacket(captureEventArgs.Packet.LinkLayerType, captureEventArgs.Packet.Data);
            PhysicalAddress packetMac = (packet as EthernetPacket).SourceHardwareAddress;

            if (!packetMac.Equals(captureEventArgs.Device.MacAddress)) // Ignores packets from our own device.
                DashButtonProbed?.Invoke(this, new DashResponse
                {
                    DashMac = packetMac.ToString(),
                    DashId = packetMac.GetHashCode(),
                    Device = captureEventArgs.Device.MacAddress.ToString()
                });
        }
    }
}