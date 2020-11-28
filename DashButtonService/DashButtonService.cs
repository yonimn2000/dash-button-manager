using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using YonatanMankovich.DashButtonCore;
using YonatanMankovich.DashButtonCore.EventArguments;

namespace YonatanMankovich.DashButtonService
{
    public partial class DashButtonService : ServiceBase
    {
        private DashButtonListener DashButtonListener { get; } = new DashButtonListener();
        private FileSystemWatcher FileSystemWatcher { get; } = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory);

        public DashButtonService()
        {
            InitializeComponent();
            DashButtonListener.OnNetworkListenerStarted += DashButtonListener_OnNetworkListenerStarted;
            DashButtonListener.OnDashButtonClicked += DashButtonListener_OnDashButtonClicked;
            DashButtonListener.OnActionExceptionThrown += DashButtonListener_OnActionExceptionThrown;
            DashButtonListener.OnExceptionThrown += DashButtonListener_OnExceptionThrown;
            DashButtonListener.LoadButtons();
            FileSystemWatcher.Changed += DashButtonFile_Changed;
            FileSystemWatcher.EnableRaisingEvents = true;
        }

        private void DashButtonFile_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Equals(DashButtonListener.DashButtonsFilePath))
            {
                try
                {
                    DashButtonListener.LoadButtons();
                    AddToLog("Reloaded dash buttons file.");
                }
                catch (IOException) { } // Ignore if the file is in use.
            }
        }

        protected override void OnStart(string[] args)
        {
            AddToLog("////////////////////////////////");
            AddToLog("Starting network listener...");
            DashButtonListener.Start();
            AddToLog(DashButtonListener.DashButtons.Count + " buttons registered.");
            AddToLog("Service started.");
        }

        protected override void OnStop()
        {
            AddToLog("Service stopped.");
        }

        public void TestInConsole(string[] args)
        {
            Console.WriteLine($"Service starting...");
            OnStart(args);
            Console.WriteLine($"Service started. Press any key to stop.");
            Console.ReadKey();
            Console.WriteLine($"Service stopping...");
            OnStop();
            Console.WriteLine($"Service stopped. Closing...");
            Thread.Sleep(1000);
        }

        private void DashButtonListener_OnExceptionThrown(object sender, ExceptionThrownEventArgs e)
        {
            AddToLog("An error has occurred: " + e.Exception.Message);
        }

        private void DashButtonListener_OnActionExceptionThrown(object sender, ActionExceptionThrownEventArgs e)
        {
            AddToLog($"An error has occurred while running the action: '{e.DashButton.ActionUrl}' " +
                $"for button '{e.DashButton.Description}'. \n{e.Exception.Message}");
        }

        private void DashButtonListener_OnDashButtonClicked(object sender, DashButtonClickedEventArgs e)
        {
            AddToLog($"Button clicked: {e.DashButton.Description} ({e.DashButton.MacAddress}) " +
                $"on {e.CaptureDeviceDescription} ({e.CaptureDeviceMacAddress})");
        }

        private void DashButtonListener_OnNetworkListenerStarted(object sender, NetworkListenerStartedEventArgs e)
        {
            AddToLog($"Started listening on {e.ListenerDevice} ({e.ListenerDeviceMacAddress})");
        }

        private void AddToLog(string message)
        {
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Log.txt", $"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}] " + message + Environment.NewLine);
        }
    }
}