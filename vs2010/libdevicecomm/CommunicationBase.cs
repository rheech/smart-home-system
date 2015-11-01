using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace libdevicecomm
{
    public enum COMMUNICATION_STANDARD
    {
        PING_TO_LEADER,
        PING_FROM_LEADER,
        LEADER_INFO,
        REQUEST_CLIENT_LIST
    }

    public class CommunicationBase
    {
        // Basic Info
        // 1. Broadcast to find a leader
        // 2-1. If there is no leader, be a leader
        // 2-2. If there is a leader, set leader information & get device list
        // 3. If finding leader request received,
        //    3-1. Ignore if you are not a leader
        //    3-2. Send information if you are a leader, and update list


        public delegate void onStatusChange(string state);
        public delegate void onConsoleMessage(string message);
        public event onStatusChange OnStatusChange;
        public event onConsoleMessage OnConsoleMessage;

        const int BROADCAST_PORT = SocketSettings.BROADCAST_PORT;

        private NetSocket _nsUDPClient, _nsUDPServer, _nsTCPClient, _nsTCPServer;
        private NetSocket _rcvClient;
        private CommunicationBaseInfo _myInfo, _leaderInfo;
        private List<CommunicationBaseInfo> _infoList;
        // http://stackoverflow.com/questions/1212742/xml-serialize-generic-list-of-serializable-objects (Serialize List)

        private int iTCPListenPort;

        public CommunicationBase()
        {
            
        }

        private void Initialize()
        {
            _myInfo = new CommunicationBaseInfo();
            _leaderInfo = new CommunicationBaseInfo();

            _nsUDPClient = new NetSocket(ProtocolType.Udp);
            _nsUDPServer = new NetSocket(ProtocolType.Udp);
            _nsTCPClient = new NetSocket(ProtocolType.Tcp);
            _nsTCPServer = new NetSocket(ProtocolType.Tcp);

            // Configure TCP Sockets
            ConfigureEventCallback();
            /*wsTCPServer.OnDataArrival += new WinSocket.onDataArrival(wsTCPServer_OnDataArrival);
            wsTCPServer.OnConnectionRequest += new WinSocket.onConnectionRequest(wsTCPServer_OnConnectionRequest);
            wsTCPServer.OnClose += new WinSocket.onClose(wsTCPServer_OnClose);
            wsTCPClient.OnConnect += new WinSocket.onConnect(wsTCPClient_OnConnect);*/
            _nsTCPServer.OnConnectionRequest += new NetSocket.cbConnectionRequest(nsTCPServer_OnConnectionRequest);
            _nsTCPServer.OnDataArrival += new NetSocket.cbDataArrival(nsTCPServer_OnDataArrival);
            _nsTCPServer.OnClose += new NetSocket.cbClose(nsTCPServer_OnClose);
            _nsTCPClient.OnConnect += new NetSocket.cbConnect(nsTCPClient_OnConnect);
        }

        private void ConfigureEventCallback()
        {

        }

        public void StartCommunication()
        {
            byte[] data;

            Initialize();

            iTCPListenPort = NetSocket.GetAvailablePort();

            // Listen TCP Server
            _nsTCPServer.Bind(iTCPListenPort);
            _nsTCPServer.Listen();

            // Print console
            RaiseEventConsoleMessage(String.Format("Listening on port {0}...", iTCPListenPort));

            // Configure myInfo
            _myInfo.info.isLeader = true;
            _myInfo.info.TCPListenPort = iTCPListenPort;
            RaiseEventConsoleMessage("Configuring my info...");

            if (OnStatusChange != null)
            {
                RaiseEventStatusChange(String.Format("Leader (Current ID: {0})", _myInfo.DeviceID));
                RaiseEventConsoleMessage("Current device set to leader.");
            }

            // Configure and send broadcast
            _nsUDPClient.Connect("255.255.255.255", SocketSettings.BROADCAST_PORT);
            data = _myInfo.Serialize();
            SendBroadcast(data);
            RaiseEventConsoleMessage("Sending broadcast.");

            // Listen for broadcast
            _nsUDPServer.ReuseAddress = true;
            _nsUDPServer.OnDataArrival += new NetSocket.cbDataArrival(nsUDPServer_OnDataArrival);
            _nsUDPServer.Bind(SocketSettings.BROADCAST_PORT);
        }

        private void SendBroadcast(byte[] data)
        {
            _nsUDPClient.SendData(CreateMessage(COMMUNICATION_STANDARD.PING_TO_LEADER, data));
        }

        private void nsUDPServer_OnDataArrival(EndPoint remoteEP, byte[] data)
        {
            switch (DispatchMessage(ref data))
            {
                case COMMUNICATION_STANDARD.PING_TO_LEADER:
                    if (_myInfo.info.isLeader)
                    {

                    }
                    break;
                default:
                    break;
            }

            CommunicationBaseInfo cbi2;
            cbi2 = CommunicationBaseInfo.FromBinary(data);

            if (cbi2 != _myInfo)
            {
                //MessageBox.Show(cbi2.DeviceID.ToString());
                RaiseEventConsoleMessage(String.Format("Received broadcast from {0}. IP address is {1}.", cbi2.DeviceID, remoteEP.ToString()));

                // Fill out remote IP address
                cbi2.info.TCPAddress = remoteEP.ToString();
                IPEndPoint ipAddress = (IPEndPoint)remoteEP;

                // Connect to a newbie (if I am a leader)
                if (_myInfo.info.isLeader)
                {
                    RaiseEventConsoleMessage(String.Format("Connecting to a new client, {0}:{1}.", remoteEP.ToString(), cbi2.info.TCPListenPort));
                    _nsTCPClient.Close();
                    _nsTCPClient.Connect(ipAddress.Address, cbi2.info.TCPListenPort);
                }
            }
        }

        private void nsTCPServer_OnConnectionRequest(Socket client)
        {
            _rcvClient = new NetSocket(client);
            _rcvClient.OnDataArrival += new NetSocket.cbDataArrival(nsTCPServer_OnDataArrival);
        }

        private void nsTCPServer_OnDataArrival(EndPoint remoteEP, byte[] data)
        {
            //MessageBox.Show(remoteEP.ToString());
        }

        private void nsTCPServer_OnClose()
        {
            //MessageBox.Show("Closed");
        }

        private void nsTCPClient_OnConnect()
        {

        }

        private void RaiseEventConsoleMessage(string message)
        {
            if (OnConsoleMessage != null)
            {
                OnConsoleMessage(message);
            }
        }

        private void RaiseEventStatusChange(string state)
        {
            if (OnStatusChange != null)
            {
                OnStatusChange(state);
            }
        }

        private static byte[] CreateMessage(COMMUNICATION_STANDARD standard, byte[] data)
        {
            int enumSize = sizeof(COMMUNICATION_STANDARD);
            byte[] dataByte = new byte[data.Length + enumSize];
            byte[] enumByte = BitConverter.GetBytes((int)standard);

            Array.Copy(enumByte, dataByte, enumSize);
            Array.Copy(data, 0, dataByte, enumSize, data.Length);

            return dataByte;
        }

        private static COMMUNICATION_STANDARD DispatchMessage(ref byte[] data)
        {
            int enumSize = sizeof(COMMUNICATION_STANDARD);
            byte[] dataByte = new byte[data.Length - enumSize];
            byte[] enumByte = new byte[enumSize];
            COMMUNICATION_STANDARD standard;

            Array.Copy(data, 0, enumByte, 0, enumSize);
            Array.Copy(data, enumSize, dataByte, 0, data.Length - enumSize);

            data = new byte[dataByte.Length];
            Array.Copy(dataByte, data, dataByte.Length);

            standard = (COMMUNICATION_STANDARD)BitConverter.ToInt32(enumByte, 0);

            return standard;
        }

        public int DeviceID
        {
            get
            {
                return _myInfo.DeviceID;
            }
        }
    }
}
