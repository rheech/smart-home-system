using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace libdevicecomm
{
    public enum MESSAGE_DIRECTIVE
    {
        REQUEST_AGENT_MODEL,
        RESPONSE_AGENT_MODEL,
        PLAIN_TEXT
    }

    public class CommunicationProtocol : CommunicationBase
    {
        public delegate byte[] cbMessageArrival(int deviceID, ENVELOPE_HEADER header, MESSAGE_DIRECTIVE directive, byte[] data);
        public event cbMessageArrival OnMessageArrival;

        public delegate string cbTextMessageArrival(int deviceID, ENVELOPE_HEADER header, string text);
        public event cbTextMessageArrival OnTextMessageArrival;

        // Based on the connection of communicationbase,
        // connect to other devices

        // Public information:
        // 

        public CommunicationProtocol()
        {

        }

        protected override byte[] OnDataArrival_lv3(IPEndPoint remoteEP, ENVELOPE_HEADER header, byte[] data)
        {
            // Read Message
            MESSAGE_DIRECTIVE directive;
            byte[] bDirective;
            byte[] bTrailer;
            int offset = 0;
            int trailerSize;

            trailerSize = data.Length - sizeof(int);
            bDirective = new byte[sizeof(int)];
            bTrailer = new byte[trailerSize];

            Array.Copy(data, offset, bDirective, 0, bDirective.Length);
            offset += bDirective.Length;

            Array.Copy(data, offset, bTrailer, 0, bTrailer.Length);
            offset += bTrailer.Length;

            directive = (MESSAGE_DIRECTIVE)BitConverter.ToInt32(bDirective, 0);

            return TranslateMessage(remoteEP, header, directive, bTrailer);
        }

        private byte[] TranslateMessage(IPEndPoint remoteEP, ENVELOPE_HEADER header, MESSAGE_DIRECTIVE directive, byte[] data)
        {
            byte[] dataToSend = null;
            string textToSend = null;

            switch (directive)
            {
                case MESSAGE_DIRECTIVE.PLAIN_TEXT:
                    if (OnTextMessageArrival != null)
                    {
                        textToSend = OnTextMessageArrival(header.MessageFrom, header, System.Text.Encoding.UTF8.GetString(data));

                        if (textToSend != null)
                        {
                            dataToSend = System.Text.Encoding.UTF8.GetBytes(textToSend);
                        }
                    }
                    break;
                default:
                    if (OnMessageArrival != null)
                    {
                        dataToSend = OnMessageArrival(header.MessageFrom, header, directive, data);
                    }
                    break;
            }

            return dataToSend;
        }

        public void Send(int deviceID, MESSAGE_DIRECTIVE directive, byte[] data)
        {
            // Create Message
            byte[] bDirective;
            byte[] bTotalData;
            int totalSize;
            int offset = 0;

            totalSize = sizeof(int) + data.Length;
            bDirective = new byte[sizeof(int)];
            bTotalData = new byte[totalSize];

            bDirective = BitConverter.GetBytes((int)directive);

            Array.Copy(bDirective, 0, bTotalData, offset, bDirective.Length);
            offset += bDirective.Length;

            Array.Copy(data, 0, bTotalData, offset, data.Length);
            offset += data.Length;

            SendDataByID(deviceID, bTotalData);
        }

        public void Send(int deviceID, string text)
        {
            Send(deviceID, MESSAGE_DIRECTIVE.PLAIN_TEXT, System.Text.Encoding.UTF8.GetBytes(text));
        }

        #region DeviceID Based Communication
        private IPEndPoint GetAddressByID(int deviceID)
        {
            foreach (CommunicationBaseInfo info in _infoList)
            {
                if (info.DeviceID == deviceID)
                {
                    return new IPEndPoint(info.NodeData.TCPAddress, info.NodeData.TCPListenPort);
                }
            }

            return null;
        }

        private int GetLeaderID()
        {
            if (_leaderInfo != null && !isLeader)
            {
                return _leaderInfo.DeviceID;
            }

            return -1;
        }

        private IPEndPoint GetLeaderAddress()
        {
            if (_leaderInfo != null && !isLeader)
            {
                return new IPEndPoint(_leaderInfo.NodeData.TCPAddress, _leaderInfo.NodeData.TCPListenPort);
            }

            return null;
        }

        private void SendDataByID(int deviceID, byte[] data)
        {
            IPEndPoint remoteEP;

            remoteEP = GetAddressByID(deviceID);

            if (remoteEP != null)
            {
                _nsTCPClient.SendDataAfterConnect(CreateEnvelope(ENVELOPE_DIRECTIVE.LV3_MESSAGE, 2, data));
                _nsTCPClient.Connect(remoteEP);
            }
            else
            {
                throw new Exception("Nonexistent ID.");
            }
        }

        public List<CommunicationBaseInfo> DeviceList
        {
            get
            {
                return _infoList;
            }
        }

        public int CurrentID
        {
            get
            {
                return _myInfo.DeviceID;
            }
        }

        public int LeaderID
        {
            get
            {
                return _leaderInfo.DeviceID;
            }
        }
        #endregion
    }
}
