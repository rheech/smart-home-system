using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace libnetsocket
{
    #region UI로 연결할 델리게이트

    /// <summary>
    /// 메시지 요청
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //public delegate void dgMessage(claClientSession session, LocalMessageEventArgs e);
    
    #endregion

    class SuperSockServer : AppServer<SuperSockClientSession, BinaryRequestInfo>
    {
        public delegate void cbMessage(SuperSockClientSession session, byte[] data);
        public event cbMessage OnMessage;

        BufferedIO bufferedIO;

        public SuperSockServer()
            : base(new DefaultReceiveFilterFactory<SuperSockReceiveFilter, BinaryRequestInfo>())
        {
            bufferedIO = new BufferedIO(SocketSettings.BUFFER_SIZE);
            bufferedIO.OnDataComplete += new BufferedIO.cbDataComplete(bufferedIO_OnDataComplete);
        }

        /// <summary>
        /// 새세션이 접속하면 발생
        /// </summary>
        /// <param name="session"></param>
        protected override void OnNewSessionConnected(SuperSockClientSession session)
        {
            //세션으로부터 받을 메시지용 이벤트
            //session.OnMessaged += session_OnMessaged;
            
            base.OnNewSessionConnected(session);
        }

        protected override void OnSessionClosed(SuperSockClientSession session, CloseReason reason)
        {
            //로그아웃 처리를 하여 유저가 끊김을 알린다.
            //OnLogoutUser(session, null);
            base.OnSessionClosed(session, reason);
        }

        /// <summary>
        /// 데이터를 받았다!
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        protected override void ExecuteCommand(SuperSockClientSession session, BinaryRequestInfo requestInfo)
        {
            bufferedIO.PushData(requestInfo.Body);

            if (OnMessage != null)
            {
                OnMessage(session, requestInfo.Body);
            }
            //

            /*//UI에 메시지를 표시한다.
            LocalMessageEventArgs e
                = new LocalMessageEventArgs(claGlobal.g_SocketUtile.ByteToString(requestInfo.Body)
                                            , Type.typeLocal.None);
            OnLog(session, e);

            //오리지널 데이터를 분석합니다.
            claSendData_Analysis insSD_A
                = new claSendData_Analysis(requestInfo.Body);


            //사용자 클래스에서 넘어온 데이터 처리
            MsgAnalysis(session, insSD_A);*/
        }

        protected void bufferedIO_OnDataComplete(byte[] data)
        {
            Console.WriteLine(System.Text.Encoding.Default.GetString(data));
        }
    }
}
