using System;

namespace YonatanMankovich.DashButtonCore.Exceptions
{
    public class PcapMissingException : Exception
    {
        public PcapMissingException() : base("WinPcap is missing. Make sure WinPcap is installed.") { }
    }
}