using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;

namespace libnetsocket.Net
{
    class SocketBinaryReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        public SocketBinaryReceiveFilter()
            : base(4)
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            byte[] headerData = new byte[length];
            Array.Copy(header, offset, headerData, 0, length);

            int headerSize = BitConverter.ToInt32(headerData, 0) - length;

            return headerSize;
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            byte[] data;
            data = new byte[length];

            Array.Copy(bodyBuffer, offset, data, 0, length);

            return new BinaryRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4), data);
        }
    }
}