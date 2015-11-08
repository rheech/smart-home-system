using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libnetsocket;
using System.Net;
using libnetsocket.Common;
using libnetsocket.Net;

namespace TestClient
{
    public partial class Form1 : Form
    {
        TCPClient client;

        public Form1()
        {
            InitializeComponent();
            client = new TCPClient();
            client.OnConnect += new cbConnect(client_OnConnect);
            client.OnDataArrival += new cbDataArrival(client_OnDataArrival);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Connect("127.0.0.1", 5000);
        }

        private void client_OnConnect()
        {

        }

        private byte[] client_OnDataArrival(IPEndPoint remoteEP, byte[] data)
        {
            MessageBox.Show(System.Text.Encoding.Default.GetString(data));
            return null;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            client.Send(System.Text.Encoding.Default.GetBytes("Hello World!Hello World!Hello World!"));
        }
    }
}
