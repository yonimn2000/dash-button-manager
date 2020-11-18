using System.ComponentModel;

namespace YonatanMankovich.DashButtonCore
{
    public class DashButton
    {
        /// <summary> When true, the action of the button will be called. </summary>
        public bool Enabled { get; set; } = true;
        
        /// <summary> The MAC address of the dash button. </summary>
        [DisplayName("Button MAC")]
        public string MacAddress { get; set; }

        /// <summary> The user description for the dash button. </summary>
        [DisplayName("Dash Button Description")]
        public string Description { get; set; }

        /// <summary> The url of the action to go to on button press. </summary>
        [DisplayName("Action URL")]
        public string ActionUrl { get; set; }
    }
}