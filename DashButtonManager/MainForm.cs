using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using YonatanMankovich.DashButtonCore;
using YonatanMankovich.DashButtonCore.EventArguments;
using YonatanMankovich.DashButtonCore.Exceptions;
using YonatanMankovich.DashButtonManager.Properties;

namespace YonatanMankovich.DashButtonManager
{
    public partial class MainForm : Form
    {
        private DashButtonListener DashButtonListener { get; } = new DashButtonListener();
        private Settings FormSettings { get; } = Settings.Default;
        private BindingList<DashButton> DashButtonsBindingList { get; }
        private const string StartWithWindowsRegistryKeyName = "Dash Button Manager";
        private RegistryKey StartWithWindowsRegistryKey { get; }
            = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

        public MainForm()
        {
            InitializeComponent();

            Size = FormSettings.FormSize;
            HorizontalSplitContainer.SplitterDistance = FormSettings.SplitterDistance;

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

        private void OnNetworkListenerStarted(object sender, NetworkListenerStartedEventArgs e)
        {
            Log($"Started listening on {e.ListenerDevice} ({e.ListenerDeviceMacAddress})");
        }

        private void OnDashButtonClicked(object sender, DashButtonClickedEventArgs e)
        {
            Log($"Button clicked: {e.DashButton.Description} ({e.DashButton.MacAddress}) " +
                $"on {e.CaptureDeviceDescription} ({e.CaptureDeviceMacAddress})");
        }

        private void Log(string message)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                DateTime now = DateTime.Now;
                LogTB.AppendText($"[{now.ToShortDateString()} {now.ToLongTimeString()}] " + message + Environment.NewLine);
            });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (StartWithWindowsRegistryKey.GetValue(StartWithWindowsRegistryKeyName) != null)
            {
                StartWithWindowsCB.Checked = true;
                BeginInvoke((MethodInvoker)MinimizeToTray);
            }

            DashButtonListener.OnNetworkListenerStarted += OnNetworkListenerStarted;
            DashButtonListener.OnDashButtonClicked += OnDashButtonClicked;
            DashButtonListener.OnActionExceptionThrown += OnActionExceptionThrown;
            DashButtonListener.OnExceptionThrown += OnExceptionThrown;

            Log("Starting network listeners...");
            Task.Run(() =>
            {
                try
                {
                    DashButtonListener.Start();
                }
                catch (DashButtonCoreException dbce)
                {
                    Log($"An error has occurred while starting the network listener: \n" + dbce.Message);
                }
            });
        }

        private void OnExceptionThrown(object sender, ExceptionThrownEventArgs e)
        {
            Log("An error has occurred: " + e.Exception.Message);
        }

        private void OnActionExceptionThrown(object sender, ActionExceptionThrownEventArgs e)
        {
            Log($"An error has occurred while running the action: '{e.DashButton.ActionUrl}' " +
                $"for button '{e.DashButton.Description}'. \n{e.Exception.Message}");
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
        }

        private void DashButtonsTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DashButtonListener.SaveButtons();
            Log("Saved dash buttons table.");
        }

        private void TrayNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            TrayNotifyIcon.Visible = false;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                MinimizeToTray();
                TrayNotifyIcon.ShowBalloonTip(1000);
            }
        }

        private void MinimizeToTray()
        {
            Hide();
            TrayNotifyIcon.Visible = true;
        }

        private void StartWithWindowsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (StartWithWindowsCB.Checked)
                StartWithWindowsRegistryKey.SetValue(StartWithWindowsRegistryKeyName, Application.ExecutablePath);
            else
                StartWithWindowsRegistryKey.DeleteValue(StartWithWindowsRegistryKeyName, false);
        }

        private void LogAllMacsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (LogAllMacsCB.Checked)
                DashButtonListener.OnMacAddressCaptured += OnMacAddressCaptured;
            else
                DashButtonListener.OnMacAddressCaptured -= OnMacAddressCaptured;
        }

        private void OnMacAddressCaptured(object sender, MacAddressCapturedEventArgs e)
        {
            Log($"MAC address captured: {e.MacAddress} on {e.CaptureDeviceDescription} ({e.CaptureDeviceMacAddress})");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormSettings.FormSize = new Size(Width, Height);
            FormSettings.SplitterDistance = HorizontalSplitContainer.SplitterDistance;
            FormSettings.Save();
        }
    }
}