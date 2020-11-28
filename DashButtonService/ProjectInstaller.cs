using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace YonatanMankovich.DashButtonService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void DashButtonServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController serviceController = new ServiceController(((ServiceInstaller)sender).ServiceName))
                serviceController.Start();
        }
    }
}