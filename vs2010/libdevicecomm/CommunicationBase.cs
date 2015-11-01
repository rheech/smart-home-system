using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace libdevicecomm
{
    public class MESSAGE_HEADER
    {
        public COMMUNICATION_STANDARD MessageType;
        public int MessageFrom;

        public MESSAGE_HEADER()
        {
        }

        public MESSAGE_HEADER(COMMUNICATION_STANDARD MessageType, int MessageFrom)
        {

        }

        public static MESSAGE_HEADER FromBinary(byte[] data)
        {
            MESSAGE_HEADER messageHeader = new MESSAGE_HEADER();
            int enumSize = sizeof(COMMUNICATION_STANDARD);
            byte[] dataByte = new byte[data.Length - enumSize];
            byte[] enumByte = new byte[enumSize];
            COMMUNICATION_STANDARD standard;

            Array.Copy(data, 0, enumByte, 0, enumSize);
            Array.Copy(data, enumSize, dataByte, 0, data.Length - enumSize);

            data = new byte[dataByte.Length];
            Array.Copy(dataByte, data, dataByte.Length);

            standard = (COMMUNICATION_STANDARD)BitConverter.ToInt32(enumByte, 0);

            messageHeader.MessageType = standard;
            messageHeader.MessageFrom = BitConverter.ToInt32(data, 0);

            return messageHeader;
        }

        public byte[] Serialize()
        {
            byte[] data = new byte[Size];
            byte[] fromData = new byte[sizeof(int)];

            int enumSize = sizeof(COMMUNICATION_STANDARD);
            int fromDataSize = sizeof(int);
            byte[] enumData = BitConverter.GetBytes((int)MessageType);
            fromData = BitConverter.GetBytes(MessageFrom);

            Array.Copy(enumData, data, enumSize);
            Array.Copy(fromData, 0, data, enumSize, fromDataSize);

            return data;
        }

        public static int Size
        {
            get
            {
                return sizeof(COMMUNICATION_STANDARD) + sizeof(int);
            }
        }
    }

    public enum COMMUNICATION_STANDARD
    {
        FIND_LEADER,
        ANSWER_LEADER,
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
            MESSAGE_HEADER header = new MESSAGE_HEADER();
            header.MessageType = COMMUNICATION_STANDARD.FIND_LEADER;
            header.MessageFrom = _myInfo.DeviceID;

            _nsUDPClient.SendData(CreateMessage(header, data));
        }

        private void nsUDPServer_OnDataArrival(EndPoint remoteEP, byte[] data)
        {
            MESSAGE_HEADER header;
            CommunicationBaseInfo cbi = null;

            // Fill out remote IP address
            IPEndPoint ipAddress = (IPEndPoint)remoteEP;

            header = DispatchMessage(ref data);

            // Truncate if it is a self-loop message
            if (header.MessageFrom != _myInfo.DeviceID)
            {
                // Get cbi if exists
                if (data.Length > 0)
                {
                    cbi = CommunicationBaseInfo.FromBinary(data);
                }

                TranslateMessage(remoteEP, header, cbi);
            }
        }

        private void TranslateMessage(EndPoint remoteEP, MESSAGE_HEADER header, CommunicationBaseInfo cbi)
        {
            IPEndPoint remoteIP = (IPEndPoint)remoteEP;

            switch (header.MessageType)
            {
                case COMMUNICATION_STANDARD.FIND_LEADER: // Find leader from newbie
                    RaiseEventConsoleMessage(String.Format("Received broadcast from {0}. IP address is {1}.", cbi.DeviceID, remoteEP.ToString()));

                    if (_myInfo.info.isLeader)
                    {
                        //replyData = _myInfo.Serialize();
                        //_nsUDPClient.SendData(CreateMessage(COMMUNICATION_STANDARD.ANSWER_LEADER, replyData));

                        // Connect to a newbie (if I am a leader)
                        RaiseEventConsoleMessage(String.Format("Connecting to a new client, {0}:{1}.", remoteEP.ToString(), cbi.info.TCPListenPort));
                        _nsTCPClient.Close();
                        _nsTCPClient.Connect(remoteIP.Address, cbi.info.TCPListenPort);
                    }
                    break;
                case COMMUNICATION_STANDARD.ANSWER_LEADER: // Answer from leader
                    // Found leader
                    RaiseEventStatusChange(String.Format("Follower (Current ID: {0})", DeviceID));
                    RaiseEventConsoleMessage("Found a leader. Changing status to a follower.");
                    _myInfo.info.isLeader = false;

                    // Fill out the leader's IP address
                    _leaderInfo.info.TCPAddress = remoteIP.Address;

                    RaiseEventConsoleMessage("Received current leader information.");
                    RaiseEventConsoleMessage(String.Format("Current leader is {0}. TCP address is {1}:{2}.",
                                    _leaderInfo.DeviceID, _leaderInfo.info.TCPAddress, _leaderInfo.info.TCPListenPort));
                    break;
                default:
                    break;
            }
        }

        private void nsTCPServer_OnConnectionRequest(Socket client)
        {
            _rcvClient = new NetSocket(client);
            _rcvClient.OnDataArrival += new NetSocket.cbDataArrival(nsTCPServer_OnDataArrival);
        }

        private void nsTCPServer_OnDataArrival(EndPoint remoteEP, byte[] data)
        {
            MESSAGE_HEADER header;
            CommunicationBaseInfo cbi = null;

            // Fill out remote IP address
            IPEndPoint ipAddress = (IPEndPoint)remoteEP;

            header = DispatchMessage(ref data);

            // Truncate if it is a self-loop message
            if (header.MessageFrom != _myInfo.DeviceID)
            {
                // Get cbi if exists
                if (data.Length > 0)
                {
                    cbi = CommunicationBaseInfo.FromBinary(data);
                    cbi.info.TCPAddress = ipAddress.Address;
                }

                TranslateMessage(remoteEP, header, cbi);
            }
        }

        private void nsTCPServer_OnClose()
        {
            //MessageBox.Show("Closed");
        }

        private void nsTCPClient_OnConnect()
        {
            byte[] tcpDataToSend;
            tcpDataToSend = CreateMessage(COMMUNICATION_STANDARD.ANSWER_LEADER, _myInfo.Serialize());
            _nsTCPClient.SendData(tcpDataToSend);
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

        private static byte[] CreateMessage_depreciated(COMMUNICATION_STANDARD standard, byte[] data)
        {
            int enumSize = sizeof(COMMUNICATION_STANDARD);
            byte[] dataByte = new byte[data.Length + enumSize];
            byte[] enumByte = BitConverter.GetBytes((int)standard);

            Array.Copy(enumByte, dataByte, enumSize);
            Array.Copy(data, 0, dataByte, enumSize, data.Length);

            return dataByte;
        }

        private byte[] CreateMessage(COMMUNICATION_STANDARD standard, byte[] data)
        {
            MESSAGE_HEADER header = new MESSAGE_HEADER();

            header.MessageFrom = _myInfo.DeviceID;
            header.MessageType = standard;

            return CreateMessage(header, data);
        }

        private static byte[] CreateMessage(MESSAGE_HEADER header, byte[] data)
        {
            int headerSize = MESSAGE_HEADER.Size;
            byte[] totalData = new byte[data.Length + headerSize];
            byte[] headerData = header.Serialize();

            Array.Copy(headerData, totalData, headerSize);
            Array.Copy(data, 0, totalData, headerSize, data.Length);

            return totalData;
        }

        private static COMMUNICATION_STANDARD DispatchMessage_depreciated(ref byte[] data)
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

        private static MESSAGE_HEADER DispatchMessage(ref byte[] data)
        {
            MESSAGE_HEADER header = new MESSAGE_HEADER();

            int headerSize = MESSAGE_HEADER.Size;
            int trailerSize = data.Length - headerSize;

            byte[] headerData = new byte[headerSize];
            byte[] trailerData = new byte[trailerSize];

            Array.Copy(data, 0, headerData, 0, headerSize);
            Array.Copy(data, headerSize, trailerData, 0, trailerSize);

            data = new byte[trailerData.Length];
            Array.Copy(trailerData, data, trailerData.Length);

            header = MESSAGE_HEADER.FromBinary(headerData);

            return header;
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
