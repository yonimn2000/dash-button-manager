
namespace YonatanMankovich.DashButtonService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DashButtonServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.DashButtonServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // DashButtonServiceProcessInstaller
            // 
            this.DashButtonServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.DashButtonServiceProcessInstaller.Password = null;
            this.DashButtonServiceProcessInstaller.Username = null;
            // 
            // DashButtonServiceInstaller
            // 
            this.DashButtonServiceInstaller.DisplayName = "Dash Button Service";
            this.DashButtonServiceInstaller.ServiceName = "DashButtonService";
            this.DashButtonServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.DashButtonServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.DashButtonServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.DashButtonServiceProcessInstaller,
            this.DashButtonServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller DashButtonServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller DashButtonServiceInstaller;
    }
}