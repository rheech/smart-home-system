﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libdeviceui
{
    public partial class frmVerifierUI : frmAbstractUI
    {
        private Size _size;

        public frmVerifierUI()
        {
            InitializeComponent();
            _size = this.Size;
        }

        public override void SetSizeDefault(Form form)
        {
            form.Size = _size;
        }

        private void frmVerifierUI_Load(object sender, EventArgs e)
        {
            txtLogConsole.WriteLine("Verification engine started.");
        }

        protected override void cp_OnConsoleMessage(string message)
        {
            txtLogConsole.WriteLine(message);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            lstOverview.Items.Add(txtQuery.Text);
        }
    }
}
