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

namespace TestClient
{
    public partial class Form1 : Form
    {
        NetSocket client;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client = new NetSocket();
            client.Connect("127.0.0.1", 5000);
            client.OnConnect += new NetSocket.cbConnect(client_OnConnect);
            client.OnDataArrival += new NetSocket.cbDataArrival(client_OnDataArrival);
        }

        private void client_OnConnect()
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            client.SendData(System.Text.Encoding.Default.GetBytes("Hello World!Hello World!Hello World!"));
        }

        private void client_OnDataArrival(EndPoint remoteEP, byte[] data)
        {
            MessageBox.Show(System.Text.Encoding.Default.GetString(data));
        }
    }
}
