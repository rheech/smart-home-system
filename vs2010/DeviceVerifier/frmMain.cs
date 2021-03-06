﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceVerifier
{
    public partial class frmMain : libdeviceui.frmVerifierUI
    {
        private const int DEVICE_ID = 40000;

        public frmMain()
        {
            InitializeComponent();
            SetSizeDefault(this);

            System.Threading.Thread.Sleep(1500);
            StartCommunication(DEVICE_ID);
        }
    }
}
