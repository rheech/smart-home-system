using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Config;

namespace libnetsocket
{
    public class TestClass
    {
        SocketServer socket;

        public TestClass()
        {
            socket = new SocketServer();
            Console.WriteLine(socket.Setup(5000).ToString());
            socket.NewRequestReceived += new RequestHandler<SocketSession, BinaryRequestInfo>(socket_NewRequestReceived);
        }

        public void Start()
        {
            socket.Start();
        }

        public void socket_NewRequestReceived(SocketSession session, BinaryRequestInfo sr)
        {
            
        }
    }
}
