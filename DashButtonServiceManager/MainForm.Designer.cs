namespace YonatanMankovich.DashButtonServiceManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.DashButtonsTable = new System.Windows.Forms.DataGridView();
            this.LogTB = new System.Windows.Forms.TextBox();
            this.HorizontalSplitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.DashButtonsTable)).BeginInit();
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
            this.DashButtonsTable.TabIndex = 1;
            this.DashButtonsTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DashButtonsTable_CellContentClick);
            this.DashButtonsTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DashButtonsTable_CellEndEdit);
            // 
            // LogTB
            // 
            this.LogTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogTB.Location = new System.Drawing.Point(0, 0);
            this.LogTB.Multiline = true;
            this.LogTB.Name = "LogTB";
            this.LogTB.ReadOnly = true;
            this.LogTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTB.Size = new System.Drawing.Size(784, 132);
            this.LogTB.TabIndex = 0;
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
            this.HorizontalSplitContainer.Size = new System.Drawing.Size(784, 361);
            this.HorizontalSplitContainer.SplitterDistance = 225;
            this.HorizontalSplitContainer.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.HorizontalSplitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Dash Button Service Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DashButtonsTable)).EndInit();
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
        private System.Windows.Forms.SplitContainer HorizontalSplitContainer;
    }
}

