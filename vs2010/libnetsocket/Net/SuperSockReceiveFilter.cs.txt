using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;

namespace libnetsocket
{
    class SuperSockReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        public SuperSockReceiveFilter()
            : base(0)
        {
            
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return (int)header[offset + 4] * 256 + (int)header[offset + 5];
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            byte[] data;
            data = new byte[length];

            Array.Copy(bodyBuffer, offset, data, 0, length);

            return new BinaryRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4), data);
        }

        /// <summary>
        /// 메시지가 정확한지 판단.
        /// </summary>
        /// <param name="readBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="toBeCopied"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        public override BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;

            //데이터가 있는지?
            if (null == readBuffer)
            {
                return NullRequestInfo;
            }

            //데이터가 있다.

            BinaryRequestInfo bri;
            byte[] data = new byte[length];

            Array.Copy(readBuffer, offset, data, 0, length);
            bri = new BinaryRequestInfo("", data);

            return bri;
        }
    }
}
