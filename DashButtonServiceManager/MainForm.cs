using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using YonatanMankovich.DashButtonCore;
using YonatanMankovich.DashButtonServiceManager.Properties;

namespace YonatanMankovich.DashButtonServiceManager
{
    public partial class MainForm : Form
    {
        private const string DashButtonServiceName = "DashButtonService";
        private Settings FormSettings { get; } = Settings.Default;
        private DashButtonListener DashButtonListener { get; } = new DashButtonListener();
        private BindingList<DashButton> DashButtonsBindingList { get; }
        private EventLogWatcher EventLogWatcher { get; }
            = new EventLogWatcher(new EventLogQuery("Application", PathType.LogName,
                $"*[System/Provider/@Name=\"{DashButtonServiceName}\"]"));

        public MainForm()
        {
            InitializeComponent();

            Size = FormSettings.FormSize;
            HorizontalSplitContainer.SplitterDistance = FormSettings.SplitterDistance;

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

        private bool IsDirectoryWritable(string directoryPath)
        {
            try
            {
                using (FileStream fs = File.Create(Path.Combine(directoryPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose)) { }
                return true;
            }
            catch
            {
                return false;
            }
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
                if (!string.IsNullOrWhiteSpace(url))
                {
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
            else if (DashButtonsTable.ReadOnly)
            {
                DialogResult dialogResult = MessageBox.Show("You need Administrative rights in order to modify values in this table. " +
                    "Would you like to restart the program with Administrative rights?",
                    "Missing Rights", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo()
                        {
                            FileName = Assembly.GetExecutingAssembly().Location,
                            UseShellExecute = true,
                            Verb = "runas"
                        });
                        Application.Exit();
                    }
                    catch (Win32Exception) { }
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
            FormSettings.FormSize = new Size(Width, Height);
            FormSettings.SplitterDistance = HorizontalSplitContainer.SplitterDistance;
            FormSettings.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!IsDirectoryWritable(AppDomain.CurrentDomain.BaseDirectory))
                DashButtonsTable.ReadOnly = true;
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
                {
                    Log(eventEntry.Message, eventEntry.TimeGenerated);
                }
                Log("Current service status: " + GetServiceStatus());
                if (DashButtonsTable.ReadOnly)
                    Log("Data editing was disabled because Dash Button Service Manager was started using a non-administrator account.");
            });
        }
    }
}