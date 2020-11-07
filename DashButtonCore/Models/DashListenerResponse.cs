using System;

namespace YonatanMankovich.DashButtonCore.Models
{
    public class DashListenerResponse : EventArgs
    {
        public bool Started { get; set; }
        public string Message { get; set; }
    }
}