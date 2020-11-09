using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using YonatanMankovich.DashButtonCore;
using YonatanMankovich.DashButtonCore.EventArguments;
using YonatanMankovich.DashButtonCore.Exceptions;

namespace YonatanMankovich.DashButtonManager
{
    public partial class MainForm : Form
    {
        private DashButtonsNetwork DashButtonsNetwork { get; } = new DashButtonsNetwork();
        private BindingList<DashButton> DashButtonsBindingList { get; }

        public MainForm()
        {
            InitializeComponent();

            DashButtonsBindingList = new BindingList<DashButton>(DashButtonsNetwork.DashButtons);
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
            DashButtonsTable.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DashButtonsTable.Columns["MacAddress"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DashButtonsTable.Columns["ActionUrl"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            // Temporary: Samples
            DashButtonsBindingList.Add(new DashButton
            {
                Enabled = true,
                Description = "Cottonelle Button; Lights On",
                MacAddress = PhysicalAddress.Parse("6C56972901EC"),
                ActionUrl = "http://smartdesk/rgb/set?r=255&g=255&b=255&s=5000"
            });
            DashButtonsBindingList.Add(new DashButton
            {
                Enabled = true,
                Description = "Bounty Button; Lights Off",
                MacAddress = PhysicalAddress.Parse("38F73D8FEA05"),
                ActionUrl = "http://smartdesk/rgb/set?r=0&g=0&b=0&s=5000"
            });
        }

        private void OnNetworkListenerStarted(object sender, NetworkListenerStartedEventArgs e)
        {
            AddToLog($"Started listening on {e.ListenerDevice} ({e.ListenerDeviceMacAddress})");
        }

        private void OnDashButtonClicked(object sender, DashButtonClickedEventArgs e)
        {
            AddToLog($"Button clicked: {e.DashButton.Description} on {e.CaptureDeviceDescription} ({e.CaptureDeviceMacAddress})");
        }

        private void AddToLog(string message)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                LogTB.AppendText($"[{DateTime.Now.ToLongTimeString()}] " + message + Environment.NewLine);
            });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DashButtonsNetwork.OnNetworkListenerStarted += OnNetworkListenerStarted;
            DashButtonsNetwork.OnDashButtonClicked += OnDashButtonClicked;
            DashButtonsNetwork.OnActionExceptionThrown += OnActionExceptionThrown;

            try
            {
                AddToLog("Starting network listeners...");
                Task.Run(() => DashButtonsNetwork.StartListening());
            }
            catch (DashButtonCoreException dbce)
            {
                AddToLog($"An error has occurred while starting the network listener: \n" + dbce.Message);
            }
        }

        private void OnActionExceptionThrown(object sender, ActionExceptionThrownEventArgs e)
        {
            AddToLog($"An error has occurred while running the action: '{e.DashButton.ActionUrl}' " +
                $"for button '{e.DashButton.Description}'. \n{e.Exception.Message}");
        }

        private async void DashButtonsTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == DashButtonsTable.Columns["Test"].Index)
                {
                    string url = DashButtonsTable.Rows[e.RowIndex].Cells["ActionUrl"].Value?.ToString();
                    try
                    {
                        await WebActionsHelpers.SendGetRequestAsync(url);
                    }
                    catch (Exception ex)
                    {
                        _ = Task.Run(() => AddToLog($"An error has occurred while running the action: '{url}' \n" + ex.Message));
                    }
                }
            }
        }
    }
}