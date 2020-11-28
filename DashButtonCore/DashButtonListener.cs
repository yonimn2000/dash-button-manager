using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using YonatanMankovich.DashButtonCore.EventArguments;

namespace YonatanMankovich.DashButtonCore
{
    public class DashButtonListener : NetworkListener
    {
        /// <summary> The collection of dash buttons. Buttons can be added to this list while the network listener is running. </summary>
        public IList<DashButton> DashButtons { get; } = new List<DashButton>();

        /// <summary> Called when a dash button is clicked. </summary>
        public event EventHandler<DashButtonClickedEventArgs> OnDashButtonClicked;

        /// <summary> Called when an exception is thrown when performing a button action. </summary>
        public event EventHandler<ActionExceptionThrownEventArgs> OnActionExceptionThrown;

        private const string saveLoadButtonsFile = "DashButtons.xml";

        public DashButtonListener()
        {
            OnMacAddressCaptured += DashButtonListener_OnMacAddressCaptured;
        }

        private async void DashButtonListener_OnMacAddressCaptured(object sender, MacAddressCapturedEventArgs macAddressCapturedEventArgs)
        {
            DashButton clickedDashButton = DashButtons
                .Where(b => b.MacAddress != null && b.Enabled && b.MacAddress.Equals(macAddressCapturedEventArgs.MacAddress))
                .FirstOrDefault();

            if (clickedDashButton == null)
                return;

            OnDashButtonClicked?.Invoke(this, new DashButtonClickedEventArgs
            {
                DashButton = clickedDashButton,
                CaptureDeviceMacAddress = macAddressCapturedEventArgs.CaptureDeviceMacAddress,
                CaptureDeviceDescription = macAddressCapturedEventArgs.CaptureDeviceDescription
            });

            try
            {
                await WebActionHelpers.SendGetRequestAsync(clickedDashButton.ActionUrl);
            }
            catch (Exception e)
            {
                OnActionExceptionThrown.Invoke(this, new ActionExceptionThrownEventArgs
                {
                    DashButton = clickedDashButton,
                    Exception = e
                });
            }
        }

        /// <summary> Saves all dash buttons to a file to later be loaded. </summary>
        public void SaveButtons()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DashButton>));
            XmlWriter xmlwriter = XmlWriter.Create(saveLoadButtonsFile, new XmlWriterSettings { Indent = true });
            serializer.Serialize(xmlwriter, DashButtons);
            xmlwriter.Close();
        }

        /// <summary> Loads all dash buttons from the saved file if it exists. </summary>
        public void LoadButtons()
        {
            if (File.Exists(saveLoadButtonsFile))
            {
                DashButtons.Clear();
                XmlSerializer serializer = new XmlSerializer(typeof(List<DashButton>));
                XmlReader xmlReader = XmlReader.Create(saveLoadButtonsFile);
                foreach (DashButton dashButton in (List<DashButton>)serializer.Deserialize(xmlReader))
                    DashButtons.Add(dashButton);
                xmlReader.Close(); 
            }
        }
    }
}