using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace libnetsocket
{
    class SocketServer : AppServer<SocketSession, BinaryRequestInfo>
    {
        public SocketServer()
            : base(new DefaultReceiveFilterFactory<SocketBinaryReceiveFilter, BinaryRequestInfo>())
        {
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }

        protected override void OnNewSessionConnected(SocketSession session)
        {
            //세션으로부터 받을 메시지용 이벤트
            //session.OnMessaged += session_OnMessaged;

            base.OnNewSessionConnected(session);
        }

        protected override void OnSessionClosed(SocketSession session, CloseReason reason)
        {
            //로그아웃 처리를 하여 유저가 끊김을 알린다.
            //OnLogoutUser(session, null);
            base.OnSessionClosed(session, reason);
        }

        protected override void ExecuteCommand(SocketSession session, BinaryRequestInfo requestInfo)
        {
            //Implement your business logic
            Console.WriteLine(System.Text.Encoding.Default.GetString(requestInfo.Body));
        }
    }
}
