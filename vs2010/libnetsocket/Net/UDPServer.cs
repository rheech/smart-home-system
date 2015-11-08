using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using libnetsocket.Common;

namespace libnetsocket.Net
{
    public class UDPServer
    {
        private const int port = 11000;

        //public delegate void onDataArrival(string groupEP, string data);
        public event cbDataArrival OnDataArrival;
        UdpClient client;

        public void Bind(int port)
        {
            client = new UdpClient(); // do not specify the address and port
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            client.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            /*Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            socket.Bind (new IPEndPoint (IPAddress.Any, port));
            client.Client = socket;

            byte[] data = new byte[260];

            socket.BeginReceive(ReceiveCallback, null);

            socket.BeginReceive(data, 0, 260, SocketFlags.Broadcast, ReceiveCallback, null);*/

            client.BeginReceive(ReceiveCallback, null);
            //client.BeginReceive(ReceiveCallback, null);

            //thread = new Thread(StartListenerInternal);
            //thread.Start();
        }

        public void Close()
        {
            if (client != null)
            {
                client.Close();
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8000);
            byte[] received = client.EndReceive(ar, ref RemoteIpEndPoint);

            if (OnDataArrival != null)
            {
                OnDataArrival(RemoteIpEndPoint, received);
                //OnDataArrival(RemoteIpEndPoint.ToString().Split(':')[0], System.Text.Encoding.Default.GetString(received));
            }

            client.BeginReceive(ReceiveCallback, null);

            //Process codes

            //MessageBox.Show(Encoding.UTF8.GetString(received));
            //Client.BeginReceive(new AsyncCallback(recv), null);
        }
    }
}
