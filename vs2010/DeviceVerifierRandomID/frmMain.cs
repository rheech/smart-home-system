using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceVerifierRandomID
{
    public partial class frmMain : libdeviceui.frmVerifierUI
    {
        private int DEVICE_ID = 40000;

        public frmMain()
        {
            InitializeComponent();
            SetSizeDefault(this);

            DEVICE_ID = new Random().Next(42000, 46000);

            System.Threading.Thread.Sleep(2000);
            StartCommunication(DEVICE_ID);
        }
    }
}
