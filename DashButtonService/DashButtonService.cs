using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using YonatanMankovich.DashButtonCore;
using YonatanMankovich.DashButtonCore.EventArguments;
using YonatanMankovich.DashButtonCore.Exceptions;

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
                    EventLog.WriteEntry("Reloaded dash buttons file.");
                }
                catch (IOException) { } // Ignore if the file is in use.
            }
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Starting network listener...");
            try
            {
                DashButtonListener.Start();
                EventLog.WriteEntry(DashButtonListener.DashButtons.Count + " buttons registered.");
            }
            catch (DashButtonCoreException e)
            {
                OnExceptionThrown(e);
            }
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
            OnExceptionThrown(e.Exception);
        }

        private void OnExceptionThrown(Exception exception)
        {
            EventLog.WriteEntry("An error has occurred: " + exception.Message, EventLogEntryType.Error);
            if (exception is PcapMissingException || exception is NoNetworkInterfacesException)
                ThreadPool.QueueUserWorkItem(_ => Stop());
        }

        private void DashButtonListener_OnActionExceptionThrown(object sender, ActionExceptionThrownEventArgs e)
        {
            EventLog.WriteEntry($"An error has occurred while running the action: '{e.DashButton.ActionUrl}' " +
                $"for button '{e.DashButton.Description}'. \n{e.Exception.Message}", EventLogEntryType.Error);
        }

        private void DashButtonListener_OnDashButtonClicked(object sender, DashButtonClickedEventArgs e)
        {
            EventLog.WriteEntry($"Button clicked: {e.DashButton.Description} ({e.DashButton.MacAddress}) " +
                $"on {e.CaptureDeviceDescription} ({e.CaptureDeviceMacAddress})");
        }

        private void DashButtonListener_OnNetworkListenerStarted(object sender, NetworkListenerStartedEventArgs e)
        {
            EventLog.WriteEntry($"Started listening on {e.ListenerDevice} ({e.ListenerDeviceMacAddress})");
        }
    }
}