using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using libnetsocket.Common;
using SuperSocket.SocketBase.Config;

namespace libnetsocket.Net
{
    public class TCPServer
    {
        public event cbDataArrival OnDataArrival;

        SocketServer _tcpServer;

        public TCPServer()
        {
            SocketSettings settings;
            settings = new SocketSettings();

            _tcpServer = new SocketServer();

            _tcpServer.NewSessionConnected += new SessionHandler<SocketSession>(tcpServer_NewSessionConnected);
            _tcpServer.NewRequestReceived += new RequestHandler<SocketSession, BinaryRequestInfo>(socket_NewRequestReceived);
            _tcpServer.SessionClosed += new SessionHandler<SocketSession,CloseReason>(tcpServer_SessionClosed);
            _tcpServer.OnExecuteCommand += new SocketServer.ExecuteCommandHandler(tcpServer_OnExecuteCommand);
        }

        public bool Setup(int port)
        {
            return _tcpServer.Setup(port);
        }

        public bool Setup(string ipAddress, int port)
        {
            return Setup(IPAddress.Parse(ipAddress), port);
        }

        public bool Setup(IPAddress ipAddress, int port)
        {
            return Setup(new IPEndPoint(ipAddress, port));
        }

        public bool Setup(IPEndPoint localEP)
        {
            ServerConfig config = new ServerConfig();
            config.Port = localEP.Port;
            config.Ip = localEP.Address.ToString();

            return _tcpServer.Setup(config);
        }

        public bool Start()
        {
            return _tcpServer.Start();
        }

        public int SessionCount
        {
            get
            {
                return _tcpServer.SessionCount;
            }
        }

        private void socket_NewRequestReceived(SocketSession session, BinaryRequestInfo requestInfo)
        {
            
        }

        private void tcpServer_NewSessionConnected(SocketSession session)
        {
            
        }

        private void tcpServer_SessionClosed(SocketSession session, CloseReason value)
        {
            
        }

        private void tcpServer_OnExecuteCommand(SocketSession session, BinaryRequestInfo requestInfo)
        {
            byte[] dataToSend = null;

            if (OnDataArrival != null)
            {
                dataToSend = OnDataArrival(session.RemoteEndPoint, requestInfo.Body);

                if (dataToSend != null)
                {
                    dataToSend = BinaryFormatter.FormatData(dataToSend);
                    session.Send(dataToSend, 0, dataToSend.Length);
                }
            }
        }
    }
}
