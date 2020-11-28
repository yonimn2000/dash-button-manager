using System;
using System.ServiceProcess;

namespace YonatanMankovich.DashButtonService
{
    static class Program
    {
        /// <summary> The main entry point for the application. </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
                new DashButtonService().TestInConsole(args);
            else
                ServiceBase.Run(new ServiceBase[]
                {
                    new DashButtonService()
                });
        }
    }
}