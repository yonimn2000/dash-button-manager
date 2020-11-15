using System.ComponentModel;

namespace YonatanMankovich.DashButtonCore
{
    public class DashButton
    {
        public bool Enabled { get; set; } = true;
        
        [DisplayName("Button MAC")]
        public string MacAddress { get; set; }

        [DisplayName("Dash Button Description")]
        public string Description { get; set; }

        [DisplayName("Action URL")]
        public string ActionUrl { get; set; }
    }
}