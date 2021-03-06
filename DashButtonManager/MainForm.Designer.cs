﻿namespace YonatanMankovich.DashButtonManager
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
            this.OptionsGB = new System.Windows.Forms.GroupBox();
            this.LogAllMacsCB = new System.Windows.Forms.CheckBox();
            this.StartWithWindowsCB = new System.Windows.Forms.CheckBox();
            this.HorizontalSplitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.DashButtonsTable)).BeginInit();
            this.OptionsGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalSplitContainer)).BeginInit();
            this.HorizontalSplitContainer.Panel1.SuspendLayout();
            this.HorizontalSplitContainer.Panel2.SuspendLayout();
            this.HorizontalSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // DashButtonsTable
            // 
            this.DashButtonsTable.AllowUserToOrderColumns = true;
            this.DashButtonsTable.AllowUserToResizeRows = false;
            this.DashButtonsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DashButtonsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DashButtonsTable.Location = new System.Drawing.Point(0, 0);
            this.DashButtonsTable.Name = "DashButtonsTable";
            this.DashButtonsTable.Size = new System.Drawing.Size(784, 225);
            this.DashButtonsTable.TabIndex = 0;
            this.DashButtonsTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DashButtonsTable_CellContentClick);
            this.DashButtonsTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DashButtonsTable_CellEndEdit);
            // 
            // LogTB
            // 
            this.LogTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogTB.Location = new System.Drawing.Point(3, 2);
            this.LogTB.Multiline = true;
            this.LogTB.Name = "LogTB";
            this.LogTB.ReadOnly = true;
            this.LogTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTB.Size = new System.Drawing.Size(648, 127);
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
            // OptionsGB
            // 
            this.OptionsGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionsGB.Controls.Add(this.LogAllMacsCB);
            this.OptionsGB.Controls.Add(this.StartWithWindowsCB);
            this.OptionsGB.Location = new System.Drawing.Point(657, 3);
            this.OptionsGB.Name = "OptionsGB";
            this.OptionsGB.Size = new System.Drawing.Size(124, 126);
            this.OptionsGB.TabIndex = 2;
            this.OptionsGB.TabStop = false;
            this.OptionsGB.Text = "Options";
            // 
            // LogAllMacsCB
            // 
            this.LogAllMacsCB.AutoSize = true;
            this.LogAllMacsCB.Location = new System.Drawing.Point(6, 42);
            this.LogAllMacsCB.Name = "LogAllMacsCB";
            this.LogAllMacsCB.Size = new System.Drawing.Size(89, 17);
            this.LogAllMacsCB.TabIndex = 1;
            this.LogAllMacsCB.Text = "Log All MACs";
            this.LogAllMacsCB.UseVisualStyleBackColor = true;
            this.LogAllMacsCB.CheckedChanged += new System.EventHandler(this.LogAllMacsCB_CheckedChanged);
            // 
            // StartWithWindowsCB
            // 
            this.StartWithWindowsCB.AutoSize = true;
            this.StartWithWindowsCB.Location = new System.Drawing.Point(6, 19);
            this.StartWithWindowsCB.Name = "StartWithWindowsCB";
            this.StartWithWindowsCB.Size = new System.Drawing.Size(117, 17);
            this.StartWithWindowsCB.TabIndex = 0;
            this.StartWithWindowsCB.Text = "Start with Windows";
            this.StartWithWindowsCB.UseVisualStyleBackColor = true;
            this.StartWithWindowsCB.CheckedChanged += new System.EventHandler(this.StartWithWindowsCB_CheckedChanged);
            // 
            // HorizontalSplitContainer
            // 
            this.HorizontalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HorizontalSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.HorizontalSplitContainer.Name = "HorizontalSplitContainer";
            this.HorizontalSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // HorizontalSplitContainer.Panel1
            // 
            this.HorizontalSplitContainer.Panel1.Controls.Add(this.DashButtonsTable);
            // 
            // HorizontalSplitContainer.Panel2
            // 
            this.HorizontalSplitContainer.Panel2.Controls.Add(this.LogTB);
            this.HorizontalSplitContainer.Panel2.Controls.Add(this.OptionsGB);
            this.HorizontalSplitContainer.Size = new System.Drawing.Size(784, 361);
            this.HorizontalSplitContainer.SplitterDistance = 225;
            this.HorizontalSplitContainer.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.HorizontalSplitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Dash Button Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.DashButtonsTable)).EndInit();
            this.OptionsGB.ResumeLayout(false);
            this.OptionsGB.PerformLayout();
            this.HorizontalSplitContainer.Panel1.ResumeLayout(false);
            this.HorizontalSplitContainer.Panel2.ResumeLayout(false);
            this.HorizontalSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalSplitContainer)).EndInit();
            this.HorizontalSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DashButtonsTable;
        private System.Windows.Forms.TextBox LogTB;
        private System.Windows.Forms.NotifyIcon TrayNotifyIcon;
        private System.Windows.Forms.GroupBox OptionsGB;
        private System.Windows.Forms.CheckBox StartWithWindowsCB;
        private System.Windows.Forms.CheckBox LogAllMacsCB;
        private System.Windows.Forms.SplitContainer HorizontalSplitContainer;
    }
}

