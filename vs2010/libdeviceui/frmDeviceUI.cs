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
    public partial class frmDeviceUI : frmAbstractUI
    {
        public frmDeviceUI()
        {
            InitializeComponent();
        }

        private void frmDeviceUI_Load(object sender, EventArgs e)
        {
        }

        protected override void cp_OnConsoleMessage(string message)
        {
            txtLogConsole.WriteLine(message);
        }

        private void btnDevList_Click(object sender, EventArgs e)
        {
            lstDeviceList.Items.Clear();

            foreach (CommunicationBaseInfo cbi in cp.DeviceList)
            {
                lstDeviceList.Items.Add(cbi.DeviceID);
            }
        }
    }
}
