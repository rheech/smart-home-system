using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libdeviceui
{
    public partial class cptConsoleTextBox : TextBox
    {
        private StringBuilder sbText;
        private delegate void onWriteLine(string text);

        public cptConsoleTextBox()
        {
            InitializeComponent();
            init();
        }

        public cptConsoleTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            init();
        }

        private void init()
        {
            sbText = new StringBuilder();
            this.Multiline = true;
            this.ReadOnly = true;
            this.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        }


        //
        // Method invocation
        // http://stackoverflow.com/questions/2367718/automating-the-invokerequired-code-pattern/
        //
        public void WriteLine(string text)
        {
            string sTemp;

            if (this.InvokeRequired)
            {
                onWriteLine OnWriteLine = new onWriteLine(WriteLine);
                this.Invoke(OnWriteLine, new object[] { text });
            }
            else
            {
                // Do anything you want with the control here
                sbText.AppendLine(text);
                sTemp = sbText.ToString();

                this.Text = sTemp.Substring(0, sTemp.Length - 1);
                this.SelectionStart = this.Text.Length;
                this.ScrollToCaret();
            }
        }
    }
}
