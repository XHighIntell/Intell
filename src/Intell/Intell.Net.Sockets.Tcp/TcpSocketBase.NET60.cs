#if NET6_0_OR_GREATER
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Intell.Net.Sockets.Tcp {

    public abstract class TcpSocketBase {

        Socket _socket;
        TcpSocketState _state = TcpSocketState.None;
        AddressFamily _addressFamily;
        byte[] _receiveBuffer = new byte[8192]; // that buffer is used for receiving 


        private TcpSocketBase() {

        }
        public TcpSocketBase(AddressFamily addressFamily): this() {
            //_socket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            _addressFamily = addressFamily;
        }
        ///<summary>Initializes a new instance of the <seealso cref="TcpSocketBase"/> class using the socket that is already connected.</summary>
        public TcpSocketBase(Socket socket): this(socket.AddressFamily) {
            _socket = socket;
            State = TcpSocketState.Connected;
            StartRecive();
        }

        public Socket InternalSocket { get { return _socket; } }

        public TcpSocketState State {
            get { return _state; }
            private set {
                if (_state != value) {
                    _state = value;
                    OnStateChanged();
                }
            }
        }


        ///<summary>Start listening to accept an incoming connection attempt.</summary>
        public void Listen(int port) {
            if (_addressFamily == AddressFamily.InterNetwork) Listen(new IPEndPoint(0, port));
            else if (_addressFamily == AddressFamily.InterNetworkV6) Listen(new IPEndPoint(IPAddress.IPv6Any, port));
            else throw new NotSupportedException("AddressFamily is not supported");
        }
        ///<summary>Start listening to accept an incoming connection attempt.</summary>
        public async void Listen(IPEndPoint endPoint) {
            Stop();
            _socket = new Socket(_addressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(endPoint);
            _socket.Listen(0);
            this.State = TcpSocketState.Listening;

            var x = new Random().Next(1000);

            while (true) {
                try {
                    var newSocket = await _socket.AcceptAsync();
                    OnAccepted(newSocket);

                    // --1-- user may stop listening right after incoming connection
                    if (_socket == null) return;

                }
                catch (SocketException exception) {
                    switch (exception.SocketErrorCode) {
                        case SocketError.OperationAborted:
                            return;
                        default: throw;
                    }
                }
            }

        }

        public void Connect(string host, int port) {
            var endpoint = new DnsEndPoint(host, port, _addressFamily);
            Connect(endpoint);
        }
        public async void Connect(EndPoint endPoint) {
            Stop();
            if (_socket == null)
                _socket = new Socket(_addressFamily, SocketType.Stream, ProtocolType.Tcp);

            //var p = _socket.Poll(2 * 1000 * 1000, SelectMode.SelectError);
            //.
            try {
                var task = _socket.ConnectAsync(endPoint);
                State = TcpSocketState.Connecting;
                await task;
                State = TcpSocketState.Connected;
            }
            catch (SocketException exception) {
                switch (exception.SocketErrorCode) {
                    case SocketError.OperationAborted:
                        State = TcpSocketState.None;
                        return;
                    case SocketError.TimedOut:
                        State = TcpSocketState.TimedOut;
                        return;
                    case SocketError.ConnectionRefused:
                        State = TcpSocketState.Refused;
                        return;
                    default:
                        throw;
                }
                
            }
            
            StartRecive();
        }

        public async void StartRecive() {
            // start reciving the connected socket
            // 1. 
            // 2. disconnected on the correct way
            // 3. disconnected in other ways:
            //      a. OperationAborted: we disposed us
            //          The overlapped operation was aborted due to the closure of the System.Net.Sockets.Socket.
            //          The I/O operation has been aborted because of either a thread exit or an application request. 
            //      b. ConnectionReset: An existing connection was forcibly closed by the remote host.
            // 
            while (true) {
                try {
                    // --1--
                    var size = await _socket.ReceiveAsync(_receiveBuffer, SocketFlags.None);
                    
                    if (size != 0) OnReceived(_receiveBuffer, 0, size);
                    
                    var r = _socket.Poll(0, SelectMode.SelectRead);
                    if (r == true && _socket.Available == 0) {
                        // closed, reset or terminated
                        _socket.Disconnect(false);
                        State = TcpSocketState.None;
                        return;
                    }

                    // --2--
                    if (size == 0) {
                        var w = _socket.Poll(0, SelectMode.SelectWrite);
                        State = TcpSocketState.None;
                        return;
                    }
                }
                catch (SocketException exception) {
                    // --3a--
                    if (exception.SocketErrorCode == SocketError.OperationAborted) {
                        return;
                    }
                    // --3b--
                    else if (exception.SocketErrorCode == SocketError.ConnectionReset) {
                        State = TcpSocketState.None; return;
                    }
                    // --3c--
                    else if (exception.SocketErrorCode == SocketError.SocketError) {
                        //State = TcpSocketState.Error;
                        throw;
                    }
                    else {
                        throw;
                    }
                    return;
                }
                
            }
            
        }

        protected void Send(byte[] buffer) { Send(buffer, 0, buffer.Length); }
        protected void Send(byte[] buffer, int offset, int count) {
            _socket.SendAsync(new ArraySegment<byte>(buffer, offset, count), SocketFlags.None);
        }
        protected void Send(IList<ArraySegment<byte>> buffers) {
            _socket.SendAsync(buffers, SocketFlags.None);
        }


        public void Shutdown() { 
            
        }

        ///<summary>Stop current action of socket.</summary>
        public void Stop() {
            if (_socket == null) return;

            if (_state == TcpSocketState.Listening) {
                _socket.Dispose();
                _socket = null; 
                State = TcpSocketState.None;
            }
            else {
                //_socket.Shutdown(SocketShutdown.Both);
                //_socket.Disconnect(true);
                _socket.Dispose();
                _socket = null;
                State = TcpSocketState.None;
            }
            //_socket.Poll(1000, SelectMode.SelectRead)

            //_socket.Dispose();
            //_socket = null;
            //State = TcpSocketState.None;
        }



        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnAccepted(Socket newSocket);
        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnReceived(byte[] buffer, int offset, int size);
        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnStateChanged();
        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnDisconnected();
    }
}

#endif