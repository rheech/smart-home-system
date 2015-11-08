using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SuperSocket.ClientEngine;
using libnetsocket.Common;

namespace libnetsocket.Net
{
    /*
        public delegate void cbConnect();
    public delegate byte[] cbDataArrival(IPEndPoint remoteEP, byte[] data);
    public delegate void cbClose();
    public delegate void cbError();
    public delegate void cbNewSession();
    public delegate void cbSessionClose();
     */
 
    public class TCPClient
    {
        public event cbConnect OnConnect;
        public event cbClose OnClose;
        public event cbDataArrival OnDataArrival;
        public event cbError OnError;

        private byte[] _dataToSend;
        private AsyncTcpSession _tcpSession;
        private IPEndPoint _remoteEP;

        public TCPClient()
        {
            _dataToSend = null;
        }

        ~TCPClient()
        {
            if (_tcpSession != null && _tcpSession.IsConnected)
            {
                _tcpSession.Close();
            }
        }

        private void initialize(IPEndPoint remoteEP)
        {
            _remoteEP = remoteEP;
            _tcpSession = new AsyncTcpSession(remoteEP);

            _tcpSession.Connected += new EventHandler(tcpSession_Connected);
            _tcpSession.Closed += new EventHandler(tcpSession_Closed);
            _tcpSession.Error += new EventHandler<ErrorEventArgs>(tcpSession_Error);
            _tcpSession.DataReceived += new EventHandler<SuperSocket.ClientEngine.DataEventArgs>(tcpSession_DataReceived);
        }

        public void SendDataAfterConnect(byte[] data)
        {
            _dataToSend = data;
        }

        public void Connect(IPEndPoint remoteEP)
        {
            initialize(remoteEP);
            Connect();
        }

        public void Connect(IPAddress ipAddress, int port)
        {
            initialize(new IPEndPoint(ipAddress, port));
            Connect();
        }

        public void Connect(string ipAddress, int port)
        {
            initialize(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            Connect();
        }

        public void Connect()
        {
            _tcpSession.Connect();
        }

        public void Send(byte[] data)
        {
            byte[] formatData;
            formatData = BinaryFormatter.FormatData(data);

            _tcpSession.Send(formatData, 0, formatData.Length);
        }

        public void Close()
        {
            if (_tcpSession != null)
            {
                _tcpSession.Close();
            }
        }

        public bool IsConnected
        {
            get
            {
                if (_tcpSession != null)
                {
                    return _tcpSession.IsConnected;
                }

                return false;
            }
        }

        private void tcpSession_Connected(object sender, EventArgs e)
        {
            // send data after connect
            if (_dataToSend != null)
            {
                this.Send(_dataToSend);
                _dataToSend = null;
            }

            if (OnConnect != null)
            {
                OnConnect();
            }
        }

        private void tcpSession_Closed(object sender, EventArgs e)
        {
            if (OnClose != null)
            {
                OnClose();
            }
        }

        private void tcpSession_Error(object sender, ErrorEventArgs e)
        {
            if (OnError != null)
            {
                OnError(e.Exception);
            }
        }

        private void tcpSession_DataReceived(object sender, DataEventArgs e)
        {
            byte[] data = null;

            if (OnDataArrival != null)
            {
                data = OnDataArrival(_remoteEP, BinaryFormatter.GetData(e.Data));

                if (data != null)
                {
                    this.Send(data);
                }
            }
        }
    }
}