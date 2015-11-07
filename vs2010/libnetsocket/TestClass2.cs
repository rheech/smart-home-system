using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libnetsocket.Client;
using SuperSocket.ClientEngine;
using System.Net;

namespace libnetsocket
{
    public class TestClass2
    {
        AsyncTcpSession client;

        public TestClass2()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

            client = new AsyncTcpSession(ep);
            
            client.DataReceived += new EventHandler<SuperSocket.ClientEngine.DataEventArgs>(tcp_DataReceived);
            client.Connected += new EventHandler(client_Connected);
        }

        public void Start()
        {
            client.Connect();
        }

        private void client_Connected(object sender, EventArgs args)
        {
        }

        private void tcp_DataReceived(object sender, DataEventArgs args)
        {

        }
    }
}
