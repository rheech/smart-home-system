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

namespace TestServer
{
    public partial class Form1 : Form
    {
        TCPServer server;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server = new TCPServer();
            server.Start(5000);
            server.OnDataReceived += new TCPServer.cbDataReceived(server_OnDataReceived);
        }

        private byte[] server_OnDataReceived(EndPoint remoteEP, byte[] data)
        {

            return System.Text.Encoding.Default.GetBytes("Hello World?");
        }
    }
}
