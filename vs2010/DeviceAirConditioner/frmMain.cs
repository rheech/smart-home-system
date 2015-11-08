using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceAirConditioner
{
    public partial class frmMain : libdeviceui.frmDeviceUI
    {
        private const int DEVICE_ID = 10000;

        public frmMain()
        {
            InitializeComponent();
            SetSizeDefault(this);

            StartCommunication(DEVICE_ID);
        }
    }
}
