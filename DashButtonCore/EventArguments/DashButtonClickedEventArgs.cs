using System;
using System.Net.NetworkInformation;

namespace YonatanMankovich.DashButtonCore.EventArguments
{
    public class DashButtonClickedEventArgs : EventArgs
    {
        public DashButton DashButton { get; internal set; }
        public PhysicalAddress CaptureDeviceMacAddress { get; internal set; }
        public string CaptureDeviceDescription { get; internal set; }
    }
}