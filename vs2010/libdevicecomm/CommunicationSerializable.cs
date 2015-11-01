using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace libdevicecomm
{
    [Serializable]
    public class CommunicationSerializable<T>
    {
        public byte[] Serialize()
        {
            return Serialize(this);
        }

        public static T FromBinary(byte[] data)
        {
            return Deserialize(data);
        }

        private static byte[] Serialize(CommunicationSerializable<T> communicationBaseParser)
        {
            MemoryStream ms;
            BinaryFormatter bf = new BinaryFormatter();

            ms = new MemoryStream();
            byte[] data;

            bf.Context = new StreamingContext();
            bf.Serialize(ms, communicationBaseParser);

            data = ms.ToArray();
            ms.Close();

            return data;
        }

        private static T Deserialize(byte[] data)
        {
            // http://stackoverflow.com/questions/7442164/c-sharp-and-net-how-to-serialize-a-structure-into-a-byte-array-using-binary
            // http://stackoverflow.com/questions/6586925/deserializing-a-byte-array

            /* var set = new DataSet();
            try
            {
                var content = StringToBytes(s);
                var formatter = new BinaryFormatter();
                using (var ms = new MemoryStream(content))
                {
                    using (var ds = new DeflateStream(ms, CompressionMode.Decompress, true))
                    {
                        set = (DataSet)formatter.Deserialize(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                // removed error handling logic!
            }*/

            T cbp;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms;

            ms = new MemoryStream(data);
            cbp = (T)Convert.ChangeType(bf.Deserialize(ms), typeof(T));

            ms.Close();

            return cbp;
        }

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/0a5aa658-7276-42e5-9d9e-b786694d6020/c-serialize-liststring-to-xml?forum=csharpgeneral
        public static byte[] Serialize(List<T> list)
        {
            byte[] rtnData;
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            MemoryStream ms = new MemoryStream();

            serializer.Serialize(ms, list);
            rtnData = ms.ToArray();

            ms.Close();

            return rtnData;
        }

        public static void Deserialize(byte[] data, ref List<T> list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            MemoryStream ms = new MemoryStream(data);

            list = new List<T>();
            List<T> other = (List<T>)(serializer.Deserialize(ms));
            list.Clear();
            list.AddRange(other);

            ms.Close();
        }
    }
}
