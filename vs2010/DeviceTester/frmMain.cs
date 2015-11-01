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
    public partial class frmMain : libdeviceui.frmAbstractUI
    {
        public frmMain()
        {
            InitializeComponent();
            StartCommunication();
        }
    }
}
