using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using YonatanMankovich.DashButtonCore;
using YonatanMankovich.DashButtonCore.EventArguments;
using YonatanMankovich.DashButtonCore.Exceptions;

namespace YonatanMankovich.DashButtonManager
{
    public partial class MainForm : Form
    {
        private DashButtonsNetwork DashButtonsNetwork { get; } = new DashButtonsNetwork();
        private BindingList<DashButton> DashButtonsBindingList { get; }
        private const string ButtonsTableFilePath = "DashButtons.xml";

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
            DashButtonsTable.Columns["MacAddress"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DashButtonsTable.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DashButtonsTable.Columns["ActionUrl"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (File.Exists(ButtonsTableFilePath))
                LoadButtons();
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
            DashButtonsNetwork.OnExceptionThrown += OnExceptionThrown;

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

        private void OnExceptionThrown(object sender, ExceptionThrownEventArgs e)
        {
            AddToLog("An error has occured: " + e.Exception.Message);
        }

        private void OnActionExceptionThrown(object sender, ActionExceptionThrownEventArgs e)
        {
            AddToLog($"An error has occurred while running the action: '{e.DashButton.ActionUrl}' " +
                $"for button '{e.DashButton.Description}'. \n{e.Exception.Message}");
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
                    AddToLog($"Testing action for {buttonDescription} button.");
                    await WebActionsHelpers.SendGetRequestAsync(url);
                }
                catch (Exception ex)
                {
                    _ = Task.Run(() => AddToLog($"An error has occurred while running the action: '{url}' \n" + ex.Message));
                }
            }
        }

        private void SaveButtons()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DashButton>));
            XmlWriter xmlwriter = XmlWriter.Create(ButtonsTableFilePath, new XmlWriterSettings { Indent = true });
            serializer.Serialize(xmlwriter, DashButtonsNetwork.DashButtons);
            xmlwriter.Close();
        }

        private void LoadButtons()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DashButton>));
            XmlReader xmlReader = XmlReader.Create(ButtonsTableFilePath);
            foreach (DashButton dashButton in (List<DashButton>)serializer.Deserialize(xmlReader))
                DashButtonsBindingList.Add(dashButton);
            xmlReader.Close();
        }

        private void DashButtonsTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SaveButtons();
            AddToLog("Saved");
        }
    }
}