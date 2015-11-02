using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace libdevicecomm
{
    public class NetSocketException : Exception
    {
        public NetSocketException()
            : base()
        {
        }

        public NetSocketException(string message)
            : base(message)
        {
        }
    }

    class BufferedIO
    {
        public delegate void cbDataComplete(byte[] data);
        public event cbDataComplete OnDataComplete;

        private int _bufferSize;
        private int _header;
        private byte[] _cumulatedData;

        public BufferedIO(int bufferSize)
        {
            _bufferSize = bufferSize;
        }

        public void PushData(byte[] data)
        {
            if (_cumulatedData == null)
            {
                byte[] headerData;

                _cumulatedData = data;

                headerData = new byte[4];
                Array.Copy(_cumulatedData, 0, headerData, 0, 4);

                _header = BitConverter.ToInt32(headerData, 0);
            }
            else
            {
                int prevLength = _cumulatedData.Length;
                Array.Resize(ref _cumulatedData, prevLength + _bufferSize);
                Array.Copy(data, 0, _cumulatedData, prevLength, _bufferSize);
            }

            ProcessCompletedData();
        }

        private void ProcessCompletedData()
        {
            byte[] completedData;
            byte[] nextData;
            int formatSize;
            int nextDataSize;

            // if transmission complete
            if (_header <= _cumulatedData.Length)
            {
                formatSize = GetFormatSize(_cumulatedData.Length, _bufferSize);
                completedData = new byte[_header - 4];
                Array.Copy(_cumulatedData, 4, completedData, 0, _header - 4);

                nextDataSize = _cumulatedData.Length - formatSize;

                // Set next data size (if there is remaining data)
                if (nextDataSize > 0)
                {
                    nextData = new byte[nextDataSize];
                    Array.Copy(_cumulatedData, _header, nextData, 0, nextDataSize);
                    _cumulatedData = nextData;
                }
                else if (nextDataSize == 0)
                {
                    _cumulatedData = null;
                }

                if (OnDataComplete != null)
                {
                    OnDataComplete(completedData);
                }
            }
        }

        public static byte[] FormatData(byte[] unformattedData, int bufferSize)
        {
            int formatSize, realDataSize;
            byte[] formatData;

            /*formatSize = (unformattedData.Length + 4) / bufferSize;

            if ((unformattedData.Length + 4) % bufferSize > 0)
            {
                formatSize++;
            }

            formatSize *= bufferSize;*/

            realDataSize = unformattedData.Length + 4;
            formatSize = GetFormatSize(realDataSize, bufferSize);

            formatData = new byte[formatSize];

            Array.Copy(unformattedData, 0, formatData, 4, unformattedData.Length);
            Array.Copy(BitConverter.GetBytes(realDataSize), 0, formatData, 0, 4);

            return formatData;
        }

        private static int GetFormatSize(int dataSize, int bufferSize)
        {
            int formatSize;

            formatSize = dataSize / bufferSize;

            if (dataSize % bufferSize > 0)
            {
                formatSize++;
            }

            return formatSize * bufferSize;
        }
    }

    public class NetSocket
    {
        public delegate void cbClose();
        public delegate void cbConnect();
        public delegate void cbConnectionRequest(Socket client);
        public delegate void cbDataArrival(EndPoint remoteEP, byte[] data);
        public delegate void cbError(string errorInfo);

        public event cbClose OnClose;
        public event cbConnect OnConnect;
        public event cbConnectionRequest OnConnectionRequest;
        public event cbDataArrival OnDataArrival;
        public event cbError OnError;

        private struct SocketInfo
        {
            public AddressFamily addressFamily;
            public SocketType socketType;
            public ProtocolType protocolType;
        }

        private Socket _sock;
        private SocketInfo _sockInfo;

        private EndPoint _localEP, _remoteEP;

        private const int BUFFER_SIZE = 1024;
        private byte[] _dataBuffer;
        private BufferedIO _bufferedIO;
        private bool _reuseAddress;

        private const int MAX_CONNECTION_REQUEST = 255;

        public NetSocket()
        {
            Initialize(ProtocolType.Tcp);
        }

        public NetSocket(ProtocolType protocol)
        {
            Initialize(protocol);
        }

        public NetSocket(Socket socket)
        {
            if (socket.ProtocolType == ProtocolType.Tcp &&
                    socket.SocketType == SocketType.Stream)
            {
                Initialize(ProtocolType.Tcp, false);
                _sock = socket;

                Receive();
            }
        }

        ~NetSocket()
        {
            if (_sock != null)
            {
                _sock.Close();
            }
        }

        private void Initialize(ProtocolType protocolType)
        {
            Initialize(protocolType, true);
        }

        private void Initialize(ProtocolType protocolType, bool initializeSocket)
        {
            _sockInfo = new SocketInfo();
            _sockInfo.addressFamily = AddressFamily.InterNetwork;

            _dataBuffer = new byte[BUFFER_SIZE];
            //_savedBinary = new BinaryData();
            _bufferedIO = new BufferedIO(BUFFER_SIZE);
            _bufferedIO.OnDataComplete += new BufferedIO.cbDataComplete(_bufferedIO_OnDataComplete);

            if (protocolType == ProtocolType.Tcp)
            {
                _sockInfo.socketType = SocketType.Stream;
                _sockInfo.protocolType = protocolType;
            }
            else if (protocolType == ProtocolType.Udp)
            {
                _sockInfo.socketType = SocketType.Dgram;
                _sockInfo.protocolType = protocolType;
            }
            else
            {
                throw new NetSocketException("Not supported protocol.");
            }

            if (initializeSocket)
            {
                ResetSocket(_sockInfo);
            }
        }

        private void ResetSocket(SocketInfo sockInfo)
        {
            _sock = new Socket(_sockInfo.addressFamily, _sockInfo.socketType, _sockInfo.protocolType);
            //_sock.NoDelay = true;
        }

        private void ChangeProtocol(ProtocolType protocol)
        {
            if (protocol == ProtocolType.Tcp)
            {
                Initialize(ProtocolType.Tcp);
            }
            else if (protocol == ProtocolType.Udp)
            {
                Initialize(ProtocolType.Udp);
            }
            else
            {
                throw new NetSocketException("Not supported protocol.");
            }

            _sockInfo.protocolType = protocol;
        }

        public ProtocolType Protocol
        {
            get
            {
                return _sockInfo.protocolType;
            }
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Connect(IPAddress ipAddress, int port)
        {
            Connect(new IPEndPoint(ipAddress, port));
        }

        public void Connect(string remoteHost, int remotePort)
        {
            Connect(Dns.GetHostAddresses(remoteHost)[0], remotePort);
        }

        public void Connect(IPEndPoint remoteEP)
        {
            _localEP = remoteEP;

            try
            {
                _sock.BeginConnect(remoteEP, new AsyncCallback(OnConnectCallback), _sock);
            }
            catch (ObjectDisposedException ex)
            {
                if (OnError != null)
                {
                    OnError(ex.ToString());
                }
            }
        }

        private void OnConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;

            // Complete the suspended connection for use
            client.EndConnect(ar);

            // If connection succeeded, wait for data
            Receive();

            // Raise OnConnect Event
            if (OnConnect != null)
            {
                OnConnect();
            }
        }

        private void Receive()
        {
            _remoteEP = _localEP;

            if (_sockInfo.protocolType != ProtocolType.Udp)
            {
                _sock.BeginReceive(_dataBuffer, 0, BUFFER_SIZE, 0, new AsyncCallback(OnReceiveCallback), new object[] { _sock, null });
            }
            else
            {
                _sock.BeginReceiveFrom(_dataBuffer, 0, BUFFER_SIZE, 0, ref _remoteEP, new AsyncCallback(OnReceiveCallback), new object[] { _sock, _remoteEP });
            }
        }

        private void OnReceiveCallback(IAsyncResult ar)
        {
            object[] objs = (object[])ar.AsyncState;

            Socket s = (Socket)objs[0];
            EndPoint ep = (EndPoint)objs[1];

            int read;

            try
            {
                // Different task for UDP / TCP
                if (_sockInfo.protocolType != ProtocolType.Udp)
                {
                    SocketError errCode;


                    read = s.EndReceive(ar, out errCode);

                    _remoteEP = s.RemoteEndPoint;

                    if (errCode != SocketError.Success)
                    {
                        _sock.Close();

                        if (OnClose != null)
                        {
                            OnClose();
                        }

                        return;
                    }
                }
                else
                {
                    read = s.EndReceiveFrom(ar, ref ep);
                    _remoteEP = ep;
                }

                // Save received data
                if (read > 0)
                {
                    _bufferedIO.PushData(_dataBuffer);
                }

                _dataBuffer = new byte[BUFFER_SIZE];

                Receive();
            }
            catch (ObjectDisposedException ex)
            {
                if (OnError != null)
                {
                    OnError(ex.ToString());
                }
            }
        }

        public void SendData(byte[] data)
        {
            byte[] formatData = BufferedIO.FormatData(data, BUFFER_SIZE);

            _sock.BeginSend(formatData, 0, formatData.Length, 0, new AsyncCallback(OnSendCallBack), _sock);
        }

        private void OnSendCallBack(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;

            int bytesWritten = client.EndSend(ar);
        }

        public void Bind(IPEndPoint localEP)
        {
            _localEP = localEP;
            _sock.Bind(localEP);

            if (_sockInfo.protocolType == ProtocolType.Udp)
            {
                Receive();
            }
        }

        public void Bind(int localPort)
        {
            Bind(new IPEndPoint(IPAddress.Any, localPort));
        }

        public void Bind(int localPort, IPAddress localIP)
        {
            Bind(new IPEndPoint(localIP, localPort));
        }

        public void Listen()
        {
            _sock.Listen(MAX_CONNECTION_REQUEST);
            StartAccept();
        }

        private void StartAccept()
        {
            _sock.BeginAccept(new AsyncCallback(OnListenCallBack), _sock);
        }

        private void OnListenCallBack(IAsyncResult ar)
        {
            Socket server = (Socket)ar.AsyncState;
            Socket client = server.EndAccept(ar);

            if (OnConnectionRequest != null)
            {
                OnConnectionRequest(client);
            }

            StartAccept();
        }

        public void Close()
        {
            if (_sock.Connected)
            {
                _sock.Shutdown(SocketShutdown.Both);
                _sock.BeginDisconnect(false, new AsyncCallback(OnCloseCallBack), _sock);
            }

            if (_sock.IsBound)
            {
                _sock.Close(100);
            }
        }

        private void OnCloseCallBack(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;

            client.EndDisconnect(ar);
            client.Close();

            ResetSocket(_sockInfo);

            if (OnClose != null)
            {
                OnClose();
            }
        }

        private void _bufferedIO_OnDataComplete(byte[] data)
        {
            if (OnDataArrival != null)
            {
                OnDataArrival(_remoteEP, data);
            }
        }

        public bool ReuseAddress
        {
            get
            {
                if (_sockInfo.protocolType == ProtocolType.Udp)
                {
                    return _reuseAddress;
                }

                throw new NetSocketException("Only works in UDP.");
            }
            set
            {
                if (_sockInfo.protocolType == ProtocolType.Udp)
                {
                    if (value)
                    {
                        _sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                    }
                    else
                    {
                        _sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 0);
                    }

                    _reuseAddress = value;
                }
                else
                {
                    throw new NetSocketException("Only works in UDP.");
                }
            }
        }

        public static int GetAvailablePort()
        {
            int port;

            Socket sock = new Socket(AddressFamily.InterNetwork,
                         SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0)); // Pass 0 here.
            port = ((IPEndPoint)sock.LocalEndPoint).Port;
            sock.Close();

            return port;
        }
    }
}
