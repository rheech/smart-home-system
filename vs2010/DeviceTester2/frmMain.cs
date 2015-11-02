using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceTester
{
    public partial class frmMain : libdeviceui.frmDeviceUI
    {
        public frmMain()
        {
            InitializeComponent();
            System.Threading.Thread.Sleep(50);
            StartCommunication();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }
    }
}
