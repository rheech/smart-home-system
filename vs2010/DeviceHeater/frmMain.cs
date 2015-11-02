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
        private const int DEVICE_ID = 19385;

        public frmMain()
        {
            InitializeComponent();
            SetSizeDefault(this);

            System.Threading.Thread.Sleep(100);
            StartCommunication(DEVICE_ID);
        }
    }
}
