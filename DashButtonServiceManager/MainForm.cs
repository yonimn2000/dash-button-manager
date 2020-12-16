using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using YonatanMankovich.DashButtonCore;

namespace YonatanMankovich.DashButtonServiceManager
{
    public partial class MainForm : Form
    {
        private const string DashButtonServiceName = "DashButtonService";
        private EventLogWatcher EventLogWatcher { get; }
            = new EventLogWatcher(new EventLogQuery("Application", PathType.LogName, $"*[System/Provider/@Name=\"{DashButtonServiceName}\"]"));
        private DashButtonListener DashButtonListener { get; } = new DashButtonListener();
        private BindingList<DashButton> DashButtonsBindingList { get; }

        public MainForm()
        {
            InitializeComponent();

            Size = Properties.Settings.Default.FormSize;
            HorizontalSplitContainer.SplitterDistance = Properties.Settings.Default.SplitterDistance;

            EventLogWatcher.EventRecordWritten += EventRecordWritten;
            EventLogWatcher.Enabled = true;

            DashButtonListener.LoadButtons();
            DashButtonsBindingList = new BindingList<DashButton>(DashButtonListener.DashButtons);
            DashButtonsTable.DataSource = new BindingSource(DashButtonsBindingList, null);

            DataGridViewButtonColumn testButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Test",
                HeaderText = "Test",
                Text = "Test",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            DashButtonsTable.Columns.Add(testButtonColumn);

            DashButtonsTable.Columns["Enabled"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DashButtonsTable.Columns["MacAddress"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DashButtonsTable.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DashButtonsTable.Columns["ActionUrl"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private string GetServiceStatus()
        {
            switch (new ServiceController(DashButtonServiceName).Status)
            {
                case ServiceControllerStatus.Running:
                    return "Running";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.StopPending:
                    return "Stopping";
                case ServiceControllerStatus.StartPending:
                    return "Starting";
                default:
                    return "Changing";
            }
        }

        private void Log(string message) => Log(message, DateTime.Now);

        private void Log(string message, DateTime dateTime)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                LogTB.AppendText($"[{dateTime.ToShortDateString()} {dateTime.ToLongTimeString()}] "
                    + message + Environment.NewLine);
            });
        }

        private async void DashButtonsTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == DashButtonsTable.Columns["Test"].Index)
            {
                DataGridViewCellCollection rowCells = DashButtonsTable.Rows[e.RowIndex].Cells;
                string buttonDescription = rowCells["Description"].Value?.ToString();
                string url = rowCells["ActionUrl"].Value?.ToString();
                try
                {
                    Log($"Testing action for {buttonDescription} button.");
                    await WebActionHelpers.SendGetRequestAsync(url);
                }
                catch (Exception ex)
                {
                    _ = Task.Run(() => Log($"An error has occurred while running the action: '{url}' \n" + ex.Message));
                }
            }
        }

        private void DashButtonsTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DashButtonListener.SaveButtons();
            Log("Saved dash buttons table.");
        }

        private void EventRecordWritten(object obj, EventRecordWrittenEventArgs arg)
        {
            Log(arg.EventRecord.FormatDescription());
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.FormSize = new Size(Width, Height);
            Properties.Settings.Default.SplitterDistance = HorizontalSplitContainer.SplitterDistance;
            Properties.Settings.Default.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _ = Task.Run(() =>
            {
                int days = 7;
                Log($"Reading events from the last {days} days...");
                EventLog log = new EventLog("Application");
                foreach (var eventEntry in log.Entries.Cast<EventLogEntry>()
                    .Where(x => x.Source.Equals(DashButtonServiceName) && x.TimeGenerated > DateTime.Now.AddDays(-days))
                    .Select(x => new
                    {
                        x.Message,
                        x.TimeGenerated
                    }))
                    Log(eventEntry.Message, eventEntry.TimeGenerated);
                Log("Current service status: " + GetServiceStatus());
            });
        }
    }
}