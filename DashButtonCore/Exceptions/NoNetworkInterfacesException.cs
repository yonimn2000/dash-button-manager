namespace YonatanMankovich.DashButtonCore.Exceptions
{
    public class NoNetworkInterfacesException : DashSharpException
    {
        public NoNetworkInterfacesException() : base("No network interfaces were found on the current computer.") { }
    }
}