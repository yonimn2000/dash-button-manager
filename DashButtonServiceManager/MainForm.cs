using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using YonatanMankovich.DashButtonCore;

namespace YonatanMankovich.DashButtonServiceManager
{
    public partial class MainForm : Form
    {
        private EventLogWatcher EventLogWatcher { get; } 
            = new EventLogWatcher(new EventLogQuery("Application", PathType.LogName, "*[System/Provider/@Name=\"DashButtonService\"]"));
        private DashButtonListener DashButtonListener { get; } = new DashButtonListener();
        private BindingList<DashButton> DashButtonsBindingList { get; }

        public MainForm()
        {
            InitializeComponent();
            SubscribeToEventLog();

            Size = Properties.Settings.Default.FormSize;
            HorizontalSplitContainer.SplitterDistance = Properties.Settings.Default.SplitterDistance;

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

        private void Log(string message)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                LogTB.AppendText($"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}] " 
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

        private void SubscribeToEventLog()
        {
            try
            {
                EventLogWatcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(EventLogEventRead);
                EventLogWatcher.Enabled = true;
            }
            catch (EventLogReadingException e)
            {
                Log("Error reading the log: " + e.Message);
            }
        }

        private void EventLogEventRead(object obj, EventRecordWrittenEventArgs arg)
        {
            Log(arg.EventRecord.FormatDescription());
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.FormSize = new Size(Width, Height);
            Properties.Settings.Default.SplitterDistance = HorizontalSplitContainer.SplitterDistance;
            Properties.Settings.Default.Save();
        }
    }
}