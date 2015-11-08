using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net;

namespace libdevicecomm
{
    [Serializable]
    public class CommunicationBaseInfo : CommunicationSerializable<CommunicationBaseInfo>
    {
        //
        // UDP packet size is 8 bytes (min) to 65,535 bytes (max), (usually 576 bytes), so we don't need to worry about
        // the packet sequencing as long as the data is smaller than the size. (need to test about it)
        //

        //
        // First byte: message type
        // Second byte: message details
        // rest of byte: other information (such as device info, ...)
        //

        [Serializable]
        public class NodeInfo
        {
            public int TCPListenPort;
            public IPAddress TCPAddress;
            //public IPAddress SOAAddress;
            //public bool isLeader;

            //public bool Equals(
        }

        private int _deviceID;
        private NodeInfo _nodeData;

        /*public CommunicationBaseInfo()
        {
            Random rnd = new Random();
            DeviceID = rnd.Next(0, 65535);

            info = new NodeInfo();
        }*/

        public CommunicationBaseInfo(int deviceID)
        {
            DeviceID = deviceID;
            _nodeData = new NodeInfo();
        }

        public string interpretMessage()
        {
            return "UNDEFINED";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //base.GetObjectData(info, context);
        }


        public int DeviceID
        {
            get
            {
                return _deviceID;
            }
            set
            {
                _deviceID = value;
            }
        }

        public NodeInfo NodeData
        {
            get
            {
                return _nodeData;
            }
            set
            {
                _nodeData = value;
            }
        }

        public bool isTCPReady
        {
            get
            {
                if (_nodeData != null)
                {
                    return _nodeData.TCPAddress != null;
                }

                return false;
            }
        }

        public override bool Equals(object obj)
        {
            CommunicationBaseInfo tempObj;

            if (obj != null &&
                    obj.GetType() == typeof(CommunicationBaseInfo))
            {
                tempObj = (CommunicationBaseInfo)Convert.ChangeType(obj, typeof(CommunicationBaseInfo));

                return tempObj.DeviceID == this.DeviceID;
            }

            return base.Equals(obj);
        }

        public static bool operator ==(CommunicationBaseInfo lhs, CommunicationBaseInfo rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(CommunicationBaseInfo lhs, CommunicationBaseInfo rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
