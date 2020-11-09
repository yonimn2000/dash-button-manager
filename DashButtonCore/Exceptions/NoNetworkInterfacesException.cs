namespace YonatanMankovich.DashButtonCore.Exceptions
{
    public class NoNetworkInterfacesException : DashButtonCoreException
    {
        public NoNetworkInterfacesException() : base("No network interfaces were found on the current computer.") { }
    }
}