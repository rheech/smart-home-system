using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceHeater
{
    public partial class frmMain : libdeviceui.frmDeviceUI
    {
        private const int DEVICE_ID = 20000;

        public frmMain()
        {
            InitializeComponent();
            SetSizeDefault(this);

            System.Threading.Thread.Sleep(500);
            StartCommunication(DEVICE_ID);
        }
    }
}
