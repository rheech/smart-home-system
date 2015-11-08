using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libdevicecomm
{
    [Serializable]
    public class CommunicationBaseInfoList : CommunicationSerializable<CommunicationBaseInfoList>
    {
        public List<CommunicationBaseInfo> baseInfo = new List<CommunicationBaseInfo>();

        public CommunicationBaseInfoList()
        {
        }
    }
}
