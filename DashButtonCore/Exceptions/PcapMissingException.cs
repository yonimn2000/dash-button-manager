namespace YonatanMankovich.DashButtonCore.Exceptions
{
    public class PcapMissingException : DashButtonCoreException
    {
        public PcapMissingException() : base("WinPcap is missing. Make sure WinPcap is installed. WinPcap can be downloaded at winpcap.org") { }
    }
}