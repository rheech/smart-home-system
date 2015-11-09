using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libdevicecomm;

namespace libdeviceui
{
    public partial class frmDeviceUI : frmAbstractUI
    {
        public frmDeviceUI()
        {
            InitializeComponent();
            lvDeviceList.FullRowSelect = true;
        }

        private void frmDeviceUI_Load(object sender, EventArgs e)
        {
        }

        protected override void cp_OnConsoleMessage(string message)
        {
            txtLogConsole.WriteLine(message);
        }

        protected override byte[] cp_OnMessageArrival(int deviceID, ENVELOPE_HEADER header, MESSAGE_DIRECTIVE directive, byte[] data)
        {
            return base.cp_OnMessageArrival(deviceID, header, directive, data);
        }

        protected override string cp_OnTextMessageArrival(int deviceID, ENVELOPE_HEADER header, string text)
        {
            MessageBox.Show(text, cp.CurrentID.ToString());

            return null;
        }

        protected override void cp_OnDeviceListUpdate()
        {
            UpdateDeviceList();
        }

        private void btnDevList_Click(object sender, EventArgs e)
        {
            UpdateDeviceList();
        }

        private void UpdateDeviceList()
        {
            ListViewItem lvi;
            lvDeviceList.Items.Clear();

            foreach (CommunicationBaseInfo cbi in cp.DeviceList)
            {
                lvi = new ListViewItem(cbi.DeviceID.ToString());
                lvi.SubItems.Add("Undefined");
                lvi.SubItems.Add(String.Format("{0}:{1}", cbi.NodeData.TCPAddress, cbi.NodeData.TCPListenPort));

                lvDeviceList.Items.Add(lvi);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            int deviceID;

            try
            {
                if (lvDeviceList.SelectedItems.Count > 0)
                {
                    deviceID = Int32.Parse(lvDeviceList.SelectedItems[0].SubItems[0].Text);

                    cp.Send(deviceID, "Hello World!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void WriteLog(string text)
        {
            txtLogConsole.WriteLine(text);
        }
    }
}
