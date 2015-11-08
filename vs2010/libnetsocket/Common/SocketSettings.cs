using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace libnetsocket.Common
{
    public delegate void cbConnect();
    public delegate byte[] cbDataArrival(IPEndPoint remoteEP, byte[] data);
    public delegate void cbClose();
    public delegate void cbError(Exception ex);
    public delegate void cbNewSession();
    public delegate void cbSessionClose();

    class SocketSettings
    {
        public static int BUFFER_SIZE = 1022;
    }
}
