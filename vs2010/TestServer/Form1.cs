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

namespace TestServer
{
    public partial class Form1 : Form
    {
        TCPServer server;

        public Form1()
        {
            InitializeComponent();
            server = new TCPServer();

            server.Setup(5000);
            server.Start();

            server.OnDataArrival += new cbDataArrival(server_OnDataArrival);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            /*
            server = new TCPServer();
            server.Start(5000);
            server.OnDataArrival += new NetSocket2.cbDataArrival(server_OnDataReceived);*/
        }

        private byte[] server_OnDataArrival(IPEndPoint remoteEP, byte[] data)
        {
            MessageBox.Show(System.Text.Encoding.Default.GetString(data));
            return System.Text.Encoding.Default.GetBytes("Hello World?");
        }

        private byte[] server_OnDataReceived(EndPoint remoteEP, byte[] data)
        {

            return System.Text.Encoding.Default.GetBytes("Hello World?");
        }
    }
}
