using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libdevicecomm;

namespace libdeviceui
{
    public partial class frmAbstractUI : Form
    {
        protected CommunicationProtocol cp;

        public frmAbstractUI()
        {
            InitializeComponent();

            cp = new CommunicationProtocol();
            cp.OnStatusChange += new CommunicationProtocol.cbStatusChange(cp_OnStatusChange);
            cp.OnConsoleMessage += new CommunicationBase.cbConsoleMessage(cp_OnConsoleMessage);
            cp.OnMessageArrival += new CommunicationProtocol.cbMessageArrival(cp_OnMessageArrival);
            cp.OnTextMessageArrival += new CommunicationProtocol.cbTextMessageArrival(cp_OnTextMessageArrival);
            cp.OnDeviceListUpdate += new CommunicationBase.cbDeviceListUpdate(cp_OnDeviceListUpdate);

            //cp.StartCommunication();
        }

        public virtual void SetSizeDefault(Form form)
        {
            form.Size = new Size(400, 350);
        }

        public int DeviceID
        {
            get
            {
                try
                {
                    return cp.DeviceID;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public void StartCommunication()
        {
            Random rnd = new Random();
            int DeviceID = rnd.Next(0, 65535);

            cp.StartCommunication(DeviceID);
        }

        public void StartCommunication(int deviceID)
        {
            cp.StartCommunication(deviceID);
        }

        protected void cp_OnStatusChange(string status)
        {
            tsLabel.Text = status;
        }

        protected virtual void cp_OnConsoleMessage(string message)
        {
        }

        protected virtual byte[] cp_OnMessageArrival(int deviceID, ENVELOPE_HEADER header, MESSAGE_DIRECTIVE directive, byte[] data)
        {
            return null;
        }

        protected virtual string cp_OnTextMessageArrival(int deviceID, ENVELOPE_HEADER header, string text)
        {
            return null;
        }

        protected virtual void cp_OnDeviceListUpdate()
        {
        }

        private void tsMenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AbstractDeviceUI_Load(object sender, EventArgs e)
        {

        }
    }
}
