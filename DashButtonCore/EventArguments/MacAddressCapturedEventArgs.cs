using System;

namespace YonatanMankovich.DashButtonCore.EventArguments
{
    public class MacAddressCapturedEventArgs : EventArgs
    {
        public string MacAddress { get; internal set; }
        public string CaptureDeviceMacAddress { get; internal set; }
        public string CaptureDeviceDescription { get; internal set; }
    }
}