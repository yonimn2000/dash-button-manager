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
            this.DashButtonsTable = new System.Windows.Forms.DataGridView();
            this.LogTB = new System.Windows.Forms.TextBox();
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 414);
            this.Controls.Add(this.LogTB);
            this.Controls.Add(this.DashButtonsTable);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DashButtonsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DashButtonsTable;
        private System.Windows.Forms.TextBox LogTB;
    }
}

