﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace libnetsocket.Net
{
    public class SocketSession : AppSession<SocketSession, BinaryRequestInfo>
    {
        protected override void OnSessionStarted()
        {
            //this.Send("Welcome to SuperSocket Telnet Server");
        }

        /*protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            this.Send("Unknow request");
        }*/

        protected override void HandleException(Exception e)
        {
            //this.Send("Application error: {0}", e.Message);
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            //add you logics which will be executed after the session is closed
            base.OnSessionClosed(reason);
        }
    }
}