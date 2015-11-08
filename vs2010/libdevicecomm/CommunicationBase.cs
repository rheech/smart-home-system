using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Xml.Serialization;
using libnetsocket;
using libnetsocket.Net;
using libnetsocket.Common;


namespace libdevicecomm
{
    #region Message Standard
    public class ENVELOPE_HEADER
    {
        public int MessageFrom;
        public int TargetLevel;
        public ENVELOPE_DIRECTIVE MessageType;

        public ENVELOPE_HEADER()
        {
        }

        public ENVELOPE_HEADER(ENVELOPE_DIRECTIVE MessageType, int MessageFrom)
        {

        }

        public static ENVELOPE_HEADER FromBinary(byte[] data)
        {
            ENVELOPE_HEADER messageHeader = new ENVELOPE_HEADER();
            int offset = 0;
            byte[] bMessageFrom, bTargetLevel, bMessageType;
            
            bMessageFrom = new byte[sizeof(int)];
            bTargetLevel = new byte[sizeof(int)];
            bMessageType = new byte[sizeof(int)];

            Array.Copy(data, offset, bMessageFrom, 0, bMessageFrom.Length);
            offset += bMessageFrom.Length;

            Array.Copy(data, offset, bTargetLevel, 0, bTargetLevel.Length);
            offset += bTargetLevel.Length;

            Array.Copy(data, offset, bMessageType, 0, bMessageType.Length);
            offset += bMessageType.Length;

            messageHeader.MessageFrom = BitConverter.ToInt32(bMessageFrom, 0);
            messageHeader.TargetLevel = BitConverter.ToInt32(bTargetLevel, 0);
            messageHeader.MessageType = (ENVELOPE_DIRECTIVE)BitConverter.ToInt32(bMessageType, 0);

            return messageHeader;
        }

        public byte[] Serialize()
        {
            int twoIntSize = sizeof(int) * 2;
            int enumSize = sizeof(int);
            int offset = 0;
            byte[] bMessageFrom, bTargetLevel, bMessageType;
            byte[] data;

            bMessageFrom = BitConverter.GetBytes(MessageFrom);
            bTargetLevel = BitConverter.GetBytes(TargetLevel);
            bMessageType = BitConverter.GetBytes((int)MessageType);

            data = new byte[twoIntSize + enumSize];

            Array.Copy(bMessageFrom, 0, data, offset, bMessageFrom.Length);
            offset += bMessageFrom.Length;

            Array.Copy(bTargetLevel, 0, data, offset, bTargetLevel.Length);
            offset += bTargetLevel.Length;

            Array.Copy(bMessageType, 0, data, offset, bMessageType.Length);
            offset += bMessageType.Length;

            return data;
        }

        public static int Size
        {
            get
            {
                return (sizeof(int) * 3);
            }
        }
    }

    public enum ENVELOPE_DIRECTIVE
    {
        FIND_LEADER,
        ANSWER_LEADER,
        LEADER_CHANGED,
        REQUEST_CLIENT_LIST,
        RESPONSE_CLIENT_LIST,
        LV3_MESSAGE
    }
    #endregion

    public abstract class CommunicationBase
    {
        // Basic Info
        // 1. Broadcast to find a leader
        // 2-1. If there is no leader, be a leader
        // 2-2. If there is a leader, set leader information & get device list
        // 3. If finding leader request received,
        //    3-1. Ignore if you are not a leader
        //    3-2. Send information if you are a leader, and update list

        #region Variable declaration
        public delegate void onStatusChange(string state);
        public delegate void onConsoleMessage(string message);
        public event onStatusChange OnStatusChange;
        public event onConsoleMessage OnConsoleMessage;

        const int BROADCAST_PORT = SocketSettings.BROADCAST_PORT;

        protected UDPClient _nsUDPClient;
        protected UDPServer _nsUDPServer;
        protected TCPClient _nsTCPClient;
        protected TCPServer _nsTCPServer;
        //private NetSocket _rcvClient;
        //private List<NetSocket> _nsTCPServerClient;
        protected CommunicationBaseInfo _myInfo, _leaderInfo;
        protected List<CommunicationBaseInfo> _infoList;

        // http://stackoverflow.com/questions/1212742/xml-serialize-generic-list-of-serializable-objects (Serialize List)

        private int iTCPListenPort;
        #endregion

        #region Initialization
        public CommunicationBase()
        {

        }

        private void Initialize(int deviceID)
        {
            _myInfo = new CommunicationBaseInfo(deviceID);
            _leaderInfo = _myInfo;

            _nsUDPClient = new UDPClient();
            _nsUDPServer = new UDPServer();
            _nsTCPClient = new TCPClient();
            _nsTCPServer = new TCPServer();

            _infoList = new List<CommunicationBaseInfo>();

            // Configure TCP Sockets
            //ConfigureEventCallback();
            /*wsTCPServer.OnDataArrival += new WinSocket.onDataArrival(wsTCPServer_OnDataArrival);
            wsTCPServer.OnConnectionRequest += new WinSocket.onConnectionRequest(wsTCPServer_OnConnectionRequest);
            wsTCPServer.OnClose += new WinSocket.onClose(wsTCPServer_OnClose);
            wsTCPClient.OnConnect += new WinSocket.onConnect(wsTCPClient_OnConnect);*/
            //_nsTCPServer.OnConnectionRequest += new NetSocket.cbConnectionRequest(nsTCPServer_OnConnectionRequest);
            _nsTCPServer.OnDataArrival += new cbDataArrival(TCPServer_OnDataArrival);
            //_nsTCPServer.OnClose += new NetSocket.cbClose(nsTCPServer_OnClose);
            //_nsTCPServer.OnDataReceived += new TCPServer.cbDataReceived(_nsTCPServer_OnDataReceived);
            _nsTCPClient.OnConnect += new cbConnect(nsTCPClient_OnConnect);
            _nsTCPClient.OnDataArrival += new cbDataArrival(TCPServer_OnDataArrival);
        }

        public void StartCommunication(int deviceID)
        {
            byte[] data;

            Initialize(deviceID);

            iTCPListenPort = NetSocket.GetAvailablePort();

            // Listen TCP Server
            _nsTCPServer.Setup(iTCPListenPort);
            _nsTCPServer.Start();

            // Print console
            RaiseEventConsoleMessage(String.Format("Listening on port {0}...", iTCPListenPort));

            // Configure myInfo
            //_myInfo.info.isLeader = true;

            _myInfo.NodeData.TCPListenPort = iTCPListenPort;

            // temporary
            _myInfo.NodeData.TCPAddress = GetLocalIPAddress();

            // Add my info (leader)
            _infoList.Add(_myInfo);
            _leaderInfo = _myInfo;

            RaiseEventConsoleMessage("Configuring my info...");

            if (OnStatusChange != null)
            {
                RaiseEventStatusChange(String.Format("Leader (Current ID: {0})", _myInfo.DeviceID));
                RaiseEventConsoleMessage("Current device set to leader.");
            }

            // Configure and send broadcast
            _nsUDPClient.Connect("255.255.255.255", SocketSettings.BROADCAST_PORT);
            data = _myInfo.Serialize();
            
            // Send Broadcast
            /*MESSAGE_HEADER header = new MESSAGE_HEADER();
            header.MessageType = COMMUNICATION_STANDARD.FIND_LEADER;
            header.MessageFrom = _myInfo.DeviceID;*/

            SendBroadcast(ENVELOPE_DIRECTIVE.FIND_LEADER, data);

            RaiseEventConsoleMessage("Sending broadcast.");

            // Listen for broadcast
            //_nsUDPServer.ReuseAddress = true;
            _nsUDPServer.OnDataArrival += new cbDataArrival(nsUDPServer_OnDataArrival);
            _nsUDPServer.Bind(SocketSettings.BROADCAST_PORT);
        }
        #endregion

        #region TCP/IP Based Communication
        private byte[] TranslateMessage(IPEndPoint remoteEP, ENVELOPE_HEADER header, byte[] data)
        {
            CommunicationBaseInfo cbi = null;

            // Get cbi if exists
            if (header.TargetLevel == 0)
            {
                cbi = CommunicationBaseInfo.FromBinary(data);
                TranslateMessage_lv1(remoteEP, header, cbi);
            }
            else if (header.TargetLevel == 1)
            {
                return TranslateMessage_lv2(remoteEP, header, data);
            }
            else if (header.TargetLevel == 2)
            {
                return OnDataArrival_lv3(remoteEP, header, data);
            }

            return null;
        }

        private void TranslateMessage_lv1(IPEndPoint remoteEP, ENVELOPE_HEADER header, CommunicationBaseInfo cbi)
        {
            byte[] dataToSend = null;

            switch (header.MessageType)
            {
                case ENVELOPE_DIRECTIVE.FIND_LEADER: // Find leader from newbie (UDP)
                    RaiseEventConsoleMessage(String.Format("Received broadcast from {0}. IP address is {1}.", cbi.DeviceID, remoteEP.Address.ToString()));

                    // Add newbie to the list
                    _infoList.Add(cbi);

                    if (isLeader)
                    {
                        //replyData = _myInfo.Serialize();
                        //_nsUDPClient.SendData(CreateMessage(COMMUNICATION_STANDARD.ANSWER_LEADER, replyData));

                        // Connect to a newbie (if I am a leader)
                        //RaiseEventConsoleMessage(String.Format("Connecting to a new client, {0}:{1}.", remoteEP.Address.ToString(), cbi.info.TCPListenPort));

                        //_nsUDPClient.SendData(CreateMessage(COMMUNICATION_STANDARD.ANSWER_LEADER, _myInfo.Serialize()));
                        //SendBroadcast(COMMUNICATION_STANDARD.ANSWER_LEADER, _myInfo.Serialize());

                        _nsTCPClient.Close();
                        _nsTCPClient.SendDataAfterConnect(CreateEnvelope(ENVELOPE_DIRECTIVE.ANSWER_LEADER, 0, _myInfo.Serialize()));
                        _nsTCPClient.Connect(remoteEP.Address, cbi.NodeData.TCPListenPort);
                    }
                    break;
                case ENVELOPE_DIRECTIVE.ANSWER_LEADER: // Answer from leader (UDP)
                    // Found leader
                    RaiseEventStatusChange(String.Format("Follower (Current ID: {0})", DeviceID));
                    RaiseEventConsoleMessage("Found a leader. Changing status to a follower.");

                    // Broadcast changed leader info
                    if (!isLeader)
                    {
                        RaiseEventConsoleMessage("Broadcasting leader change info.");
                        SendBroadcast(ENVELOPE_DIRECTIVE.LEADER_CHANGED, _leaderInfo.Serialize());
                    }

                    //_myInfo.info.isLeader = false;

                    // Fill out the leader's IP address
                    _leaderInfo = cbi;
                    _leaderInfo.NodeData.TCPAddress = remoteEP.Address;

                    RaiseEventConsoleMessage("Received current leader information.");
                    RaiseEventConsoleMessage("Current leader is {0}. TCP address is {1}:{2}.",
                                    _leaderInfo.DeviceID, _leaderInfo.NodeData.TCPAddress, _leaderInfo.NodeData.TCPListenPort);

                    // Request Client List to the leader
                    RaiseEventConsoleMessage("Requesting client list...");
                    dataToSend = CreateEnvelope(ENVELOPE_DIRECTIVE.REQUEST_CLIENT_LIST, 1);

                    _nsTCPClient.Close();
                    _nsTCPClient.SendDataAfterConnect(dataToSend);
                    _nsTCPClient.Connect(remoteEP.Address, _leaderInfo.NodeData.TCPListenPort);
                    //byte[] clientList = CommunicationBaseInfo.Serialize(_infoList);
                    break;
                case ENVELOPE_DIRECTIVE.LEADER_CHANGED:
                    RaiseEventConsoleMessage("Found a new leader. Verifying info...");

                    if (isLeader)
                    {
                        //_myInfo.info.isLeader = false;

                        RaiseEventConsoleMessage("Received a new leader information.");
                        RaiseEventConsoleMessage("New leader is {0}. TCP address is {1}:{2}.",
                                        cbi.DeviceID, cbi.NodeData.TCPAddress, cbi.NodeData.TCPListenPort);

                        // Fill out the leader's IP address
                        _leaderInfo = cbi;
                        _leaderInfo.NodeData.TCPAddress = remoteEP.Address;
                    }
                    break;
                default:
                    break;
            }
        }

        private byte[] TranslateMessage_lv2(IPEndPoint remoteEP, ENVELOPE_HEADER header, byte[] data)
        {
            byte[] dataToSend = null;

            switch (header.MessageType)
            {
                case ENVELOPE_DIRECTIVE.REQUEST_CLIENT_LIST:
                    byte[] tempData;
                    tempData = SerializeClientList(_infoList);

                    dataToSend = CreateEnvelope(ENVELOPE_DIRECTIVE.RESPONSE_CLIENT_LIST, 1, tempData);
                    /*_nsTCPClient.SendData(CreateMessage(COMMUNICATION_STANDARD.RESPONSE_CLIENT_LIST,
                                    CommunicationBaseInfo.Serialize(_infoList)));*/
                    break;
                case ENVELOPE_DIRECTIVE.RESPONSE_CLIENT_LIST:
                    _infoList = DeserializeClientList(data);
                    RaiseEventConsoleMessage("Received client list from {0}.", header.MessageFrom);

                    break;
                default:
                    break;
            }

            return dataToSend;
        }

        protected virtual byte[] OnDataArrival_lv3(IPEndPoint remoteEP, ENVELOPE_HEADER header, byte[] data)
        {
            throw new NotImplementedException("Lv3 method is not implemented in this class.");
        }

        private static byte[] SerializeClientList(List<CommunicationBaseInfo> infoList)
        {
            CommunicationBaseInfoList cbi_list = new CommunicationBaseInfoList();
            cbi_list.baseInfo = infoList;

            byte[] clientList = cbi_list.Serialize();

            return clientList;
        }

        private static List<CommunicationBaseInfo> DeserializeClientList(byte[] data)
        {
            CommunicationBaseInfoList cbi_list;

            cbi_list = CommunicationBaseInfoList.FromBinary(data);

            return cbi_list.baseInfo;
        }

        private void SendBroadcast(ENVELOPE_DIRECTIVE messageType)
        {
            SendBroadcast(messageType, null);
        }

        private void SendBroadcast(ENVELOPE_DIRECTIVE messageType, byte[] data)
        {
            //_nsUDPClient.Connect("255.255.255.255", SocketSettings.BROADCAST_PORT);
            _nsUDPClient.Send(CreateEnvelope(messageType, 0, data));
        }

        private void nsTCPClient_OnConnect()
        {
            
        }

        private byte[] nsUDPServer_OnDataArrival(IPEndPoint remoteEP, byte[] data)
        {
            ENVELOPE_HEADER header;

            header = DispatchEnvelope(ref data);

            // Truncate if it is a self-loop message
            if (header.MessageFrom != _myInfo.DeviceID)
            {
                TranslateMessage(remoteEP, header, data);
            }

            return null;
        }

        private byte[] TCPServer_OnDataArrival(IPEndPoint remoteEP, byte[] data)
        {
            byte[] dataToSend = null;
            ENVELOPE_HEADER header;

            // Fill out remote IP address
            IPEndPoint ipAddress = (IPEndPoint)remoteEP;

            header = DispatchEnvelope(ref data);

            // Truncate if it is a self-loop message
            if (header.MessageFrom != _myInfo.DeviceID)
            {
                dataToSend = TranslateMessage(remoteEP, header, data);
            }

            return dataToSend;
        }

        private static IPAddress GetLocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
        #endregion

        #region Envelope Standard

        protected byte[] CreateEnvelope(ENVELOPE_DIRECTIVE standard, int targetLevel)
        {
            return CreateEnvelope(standard, targetLevel, null);
        }

        protected byte[] CreateEnvelope(ENVELOPE_DIRECTIVE standard, int targetLevel, byte[] data)
        {
            ENVELOPE_HEADER header = new ENVELOPE_HEADER();

            header.MessageFrom = _myInfo.DeviceID;
            header.MessageType = standard;
            header.TargetLevel = targetLevel;

            return CreateEnvelope(header, data);
        }

        protected static byte[] CreateEnvelope(ENVELOPE_HEADER header, byte[] data)
        {
            byte[] headerData, totalData;
            int headerSize, dataSize = 0;

            headerSize = ENVELOPE_HEADER.Size;

            if (data != null)
            {
                dataSize = data.Length;
            }

            totalData = new byte[headerSize + dataSize];
            headerData = header.Serialize();

            Array.Copy(headerData, totalData, headerSize);

            if (dataSize > 0)
            {
                Array.Copy(data, 0, totalData, headerSize, data.Length);
            }

            return totalData;
        }

        private static ENVELOPE_HEADER DispatchEnvelope(ref byte[] data)
        {
            ENVELOPE_HEADER header = new ENVELOPE_HEADER();

            int headerSize = ENVELOPE_HEADER.Size;
            int trailerSize = data.Length - headerSize;

            byte[] headerData = new byte[headerSize];
            byte[] trailerData = new byte[trailerSize];

            Array.Copy(data, 0, headerData, 0, headerSize);
            Array.Copy(data, headerSize, trailerData, 0, trailerSize);

            data = new byte[trailerData.Length];
            Array.Copy(trailerData, data, trailerData.Length);

            header = ENVELOPE_HEADER.FromBinary(headerData);

            return header;
        }
        #endregion

        #region Log
        private void RaiseEventConsoleMessage(string message, params object[] args)
        {
            RaiseEventConsoleMessage(String.Format(message, args));
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
        #endregion
        public int DeviceID
        {
            get
            {
                return _myInfo.DeviceID;
            }
        }

        public bool isLeader
        {
            get
            {
                if (_leaderInfo != null && _myInfo != null)
                {
                    return _myInfo == _leaderInfo;
                }

                return false;
            }
        }
    }
}
