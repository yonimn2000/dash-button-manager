using System;

namespace YonatanMankovich.DashButtonCore.EventArguments
{
    public class ExceptionThrownEventArgs : EventArgs
    {
        public Exception Exception { get; internal set; }
    }
}