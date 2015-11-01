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
            cp.OnStatusChange += new CommunicationProtocol.onStatusChange(cp_OnStatusChange);
            cp.OnConsoleMessage += new CommunicationBase.onConsoleMessage(cp_OnConsoleMessage);

            //cp.StartCommunication();
        }

        public int DeviceID
        {
            get
            {
                return cp.DeviceID;
            }
        }

        public void StartCommunication()
        {
            cp.StartCommunication();
        }

        protected void cp_OnStatusChange(string status)
        {
            tsLabel.Text = status;
        }

        protected virtual void cp_OnConsoleMessage(string message)
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
