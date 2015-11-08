using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace libnetsocket.Net
{
    public class UDPClient
    {
        UdpClient _client;
        IPEndPoint _remoteEP;

        public UDPClient()
        {
            _client = new UdpClient();
        }

        public void Connect(string ipAddress, int port)
        {
            Connect(IPAddress.Parse(ipAddress), port);
        }

        public void Connect(IPAddress ipAddress, int port)
        {
            _remoteEP = new IPEndPoint(ipAddress, port);
            Connect(_remoteEP);
        }

        public void Connect(IPEndPoint remoteEP)
        {
            _remoteEP = remoteEP;
            _client.Connect(remoteEP);
        }

        public void Send(byte[] data)
        {
            _client.BeginSend(data, data.Length, new AsyncCallback(OnSendCallBack), _client);
        }

        private void OnSendCallBack(IAsyncResult ar)
        {
            UdpClient client = (UdpClient)ar.AsyncState;

            int bytesWritten = client.EndSend(ar);
        }

        public void Close()
        {
            _client.Close();
        }

        private void aa()
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 5448);
            byte[] bytes = Encoding.ASCII.GetBytes("Foo");
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }
    }
}
