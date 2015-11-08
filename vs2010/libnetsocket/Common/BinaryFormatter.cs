using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libnetsocket.Common
{
    class BinaryFormatter
    {
        private static int HEADER_SIZE = sizeof(int);

        public static byte[] FormatData(byte[] rawData)
        {
            byte[] rtnData;
            byte[] header;
            int dataSize;

            dataSize = rawData.Length + HEADER_SIZE;
            header = BitConverter.GetBytes(dataSize);
            rtnData = new byte[rawData.Length + HEADER_SIZE];

            Array.Copy(header, 0, rtnData, 0, HEADER_SIZE);
            Array.Copy(rawData, 0, rtnData, HEADER_SIZE, rawData.Length);

            return rtnData;
        }

        public static int GetHeader(byte[] formattedData)
        {
            byte[] header;

            header = new byte[HEADER_SIZE];

            Array.Copy(formattedData, 0, header, 0, HEADER_SIZE);

            return BitConverter.ToInt32(header, 0);
        }

        public static byte[] GetData(byte[] formattedData)
        {
            byte[] rtnData;
            int dataSize;

            dataSize = GetHeader(formattedData);
            rtnData = new byte[dataSize];

            Array.Copy(formattedData, HEADER_SIZE, rtnData, 0, dataSize);

            return rtnData;
        }
    }
}