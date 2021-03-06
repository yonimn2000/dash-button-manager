﻿using System;

namespace YonatanMankovich.DashButtonCore.EventArguments
{
    public class DashButtonClickedEventArgs : EventArgs
    {
        public DashButton DashButton { get; internal set; }
        public string CaptureDeviceMacAddress { get; internal set; }
        public string CaptureDeviceDescription { get; internal set; }
    }
}