using System.ComponentModel;
using System.Net.NetworkInformation;

namespace YonatanMankovich.DashButtonCore
{
    public class DashButton
    {
        public bool Enabled { get; set; }
        
        [DisplayName("Button MAC")]
        public PhysicalAddress MacAddress { get; set; }

        [DisplayName("Dash Button Description")]
        public string Description { get; set; }

        [DisplayName("Action URL")]
        public string ActionUrl { get; set; }
    }
}