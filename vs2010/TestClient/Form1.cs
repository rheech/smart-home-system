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
        NetSocket2 client;
        TestClass2 tc;

        public Form1()
        {
            InitializeComponent();
            tc = new TestClass2();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*client = new NetSocket2();
            client.Connect("127.0.0.1", 5000);
            client.OnConnect += new NetSocket2.cbConnect(client_OnConnect);
            client.OnDataArrival += new NetSocket2.cbDataArrival(client_OnDataArrival);*/
            tc.Start();
        }

        private void client_OnConnect()
        {

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            //client.SendData(System.Text.Encoding.Default.GetBytes("Hello World!Hello World!Hello World!"));
        }

        /*private byte[] client_OnDataArrival(EndPoint remoteEP, byte[] data)
        {
            /*MessageBox.Show(System.Text.Encoding.Default.GetString(data));

            return null;
        }*/
    }
}
