using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
    }
}
