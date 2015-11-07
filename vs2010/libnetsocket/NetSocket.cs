using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace libnetsocket
{
    public delegate void cbConnect();
    public delegate byte[] cbDataArrival(IPEndPoint remoteEP, byte[] data);
    public delegate void cbClose();
    public delegate void cbError();
    public delegate void cbNewSession();
    public delegate void cbSessionClose();

    interface NetSocket
    {
        void SendData(byte[] data);
    }
}
