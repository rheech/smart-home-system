﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SocketTest
{
    class claServer : AppServer<claClientSession, BinaryRequestInfo>
    {
    }
}
