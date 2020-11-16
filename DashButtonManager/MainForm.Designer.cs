namespace YonatanMankovich.DashButtonManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.DashButtonsTable = new System.Windows.Forms.DataGridView();
            this.LogTB = new System.Windows.Forms.TextBox();
            this.TrayNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DashButtonsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // DashButtonsTable
            // 
            this.DashButtonsTable.AllowUserToOrderColumns = true;
            this.DashButtonsTable.AllowUserToResizeRows = false;
            this.DashButtonsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DashButtonsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DashButtonsTable.Location = new System.Drawing.Point(0, 0);
            this.DashButtonsTable.Name = "DashButtonsTable";
            this.DashButtonsTable.Size = new System.Drawing.Size(800, 263);
            this.DashButtonsTable.TabIndex = 0;
            this.DashButtonsTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DashButtonsTable_CellContentClick);
            this.DashButtonsTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DashButtonsTable_CellEndEdit);
            // 
            // LogTB
            // 
            this.LogTB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LogTB.Location = new System.Drawing.Point(0, 269);
            this.LogTB.Multiline = true;
            this.LogTB.Name = "LogTB";
            this.LogTB.ReadOnly = true;
            this.LogTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTB.Size = new System.Drawing.Size(800, 145);
            this.LogTB.TabIndex = 1;
            // 
            // TrayNotifyIcon
            // 
            this.TrayNotifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TrayNotifyIcon.BalloonTipText = "Working in the background now.";
            this.TrayNotifyIcon.BalloonTipTitle = "Dash Button Manager";
            this.TrayNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayNotifyIcon.Icon")));
            this.TrayNotifyIcon.Text = "Dash Button Manager";
            this.TrayNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TrayNotifyIcon_MouseClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 414);
            this.Controls.Add(this.LogTB);
            this.Controls.Add(this.DashButtonsTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Dash Button Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.DashButtonsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DashButtonsTable;
        private System.Windows.Forms.TextBox LogTB;
        private System.Windows.Forms.NotifyIcon TrayNotifyIcon;
    }
}

