using System;

namespace YonatanMankovich.DashButtonCore.EventArguments
{
    public class ActionExceptionThrownEventArgs : EventArgs
    {
        public DashButton DashButton { get; internal set; }
        public Exception Exception { get; internal set; }
    }
}