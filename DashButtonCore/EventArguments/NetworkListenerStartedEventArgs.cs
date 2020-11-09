using System;
using System.Net.NetworkInformation;

namespace YonatanMankovich.DashButtonCore.EventArguments
{
    public class NetworkListenerStartedEventArgs : EventArgs
    {
        public string ListenerDevice { get; internal set; }
        public PhysicalAddress ListenerDeviceMacAddress { get; internal set; }
    }
}