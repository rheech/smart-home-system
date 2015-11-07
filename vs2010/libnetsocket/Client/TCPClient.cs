using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SuperSocket.ClientEngine;

namespace libnetsocket.Client
{
    class TCPClient
    {
        SuperSocket.ClientEngine.AsyncTcpSession tcp;
        public TCPClient()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

            tcp = new SuperSocket.ClientEngine.AsyncTcpSession(ep);

            tcp.Connect();

            tcp.DataReceived += new EventHandler<SuperSocket.ClientEngine.DataEventArgs>(tcp_DataReceived);

        }

        private void tcp_DataReceived(object sender, DataEventArgs args)
        {
            
        }
    }
}
