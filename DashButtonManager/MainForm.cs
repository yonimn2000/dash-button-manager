using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YonatanMankovich.DashButtonCore;
using YonatanMankovich.DashButtonCore.Exceptions;
using YonatanMankovich.DashButtonCore.Models;

namespace YonatanMankovich.DashButtonManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Console.WriteLine("Dash Buttons have two MAC addresses, their wakeup and their pair. The last one you receive is the pair address. ");
            var network = new DashNetwork();
            network.ListenerStarted += Network_ListenerStarted;
            network.DashButtonProbed += Network_DashProbed;
            try
            {
                network.StartListening();
            }
            catch (DashSharpException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Network_ListenerStarted(object sender, EventArgs e)
        {
            Console.WriteLine(((DashListenerResponse)e).Message);
        }

        private static void Network_DashProbed(object sender, EventArgs e)
        {
            var probe = (DashResponse)e;
            Console.WriteLine("Amazon Dash Connected: " + probe.DashMac + " seen on " + probe.Device);
        }
    }
}