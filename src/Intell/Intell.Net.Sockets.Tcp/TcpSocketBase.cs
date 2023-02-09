using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Intell.Net.Sockets.Tcp {
    ///<summary>Implements generic TCP Socket for both IPv4 and IPv6.</summary>
    public abstract class TcpSocketBase {
        // private fields
        Socket _socket;
        TcpSocketState _state = TcpSocketState.None;
        readonly AddressFamily _addressFamily;
        readonly byte[] _receiveBuffer = new byte[8192]; // that buffer is used for receiving 


        
        ///<summary>Initializes a new instance of the <seealso cref="TcpSocketBase"/> class.</summary>
        public TcpSocketBase(AddressFamily addressFamily) { _addressFamily = addressFamily; }
        ///<summary>Initializes a new instance of the <seealso cref="TcpSocketBase"/> class using the socket that is already connected.</summary>
        public TcpSocketBase(Socket socket) : this(socket.AddressFamily) {
            _socket = socket;
            State = TcpSocketState.Connected;
            StartRecive();
        }

        #region properties
        public Socket InternalSocket { get { return _socket; } }
        public TcpSocketState State {
            get { return _state; }
            private set {
                if (_state != value) {
                    var old = _state;
                    _state = value;
                    OnStateChanged(old, _state);
                }
            }
        }
        public AddressFamily AddressFamily { get { return _addressFamily; } }
        #endregion


        ///<summary>Start listening to accept an incoming connection attempt.</summary>
        public void Listen(int port) {
            if (_addressFamily == AddressFamily.InterNetwork) Listen(new IPEndPoint(0, port));
            else if (_addressFamily == AddressFamily.InterNetworkV6) Listen(new IPEndPoint(IPAddress.IPv6Any, port));
            else throw new NotSupportedException("AddressFamily is not supported");
        }
        ///<summary>Start listening to accept an incoming connection attempt.</summary>
        public void Listen(IPEndPoint endPoint) {
            Stop();
            _socket = new Socket(_addressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(endPoint);
            _socket.Listen(0);
            this.State = TcpSocketState.Listening;
            
            var e = new SocketAsyncEventArgs();
            e.Completed += acceptCallback;
            _socket.AcceptAsync(e);
        }

        public void Connect(string host, int port) {
            var endpoint = new DnsEndPoint(host, port, _addressFamily);
            Connect(endpoint);
        }
        public void Connect(EndPoint endPoint) {
            Stop();
            if (_socket == null) _socket = new Socket(_addressFamily, SocketType.Stream, ProtocolType.Tcp);

            var e = new SocketAsyncEventArgs();
            e.UserToken = this;
            e.Completed += connectCallback;
            e.RemoteEndPoint = endPoint;
            _socket.ConnectAsync(e);

            State = TcpSocketState.Connecting;
        }
        public void StartRecive() {
            // start reciving the connected socket
            // 1. 
            // 2. disconnected on the correct way
            // 3. disconnected in other ways:
            //      a. OperationAborted: we disposed us
            //          The overlapped operation was aborted due to the closure of the System.Net.Sockets.Socket.
            //          The I/O operation has been aborted because of either a thread exit or an application request. 
            //      b. ConnectionReset: An existing connection was forcibly closed by the remote host.
            // 

            var e = new SocketAsyncEventArgs();
            e.SetBuffer(_receiveBuffer, 0, _receiveBuffer.Length);
            e.Completed += receiveCallback;
            _socket.ReceiveAsync(e);
        }


        protected void Send(byte[] buffer) { Send(buffer, 0, buffer.Length); }
        protected void Send(byte[] buffer, int offset, int count) {
            //_socket.BeginSend(buffer, offset, count, SocketFlags.None, sendCallback, _socket);

            var e = new SocketAsyncEventArgs();
            e.SetBuffer(buffer, offset, count);
            e.Completed += sendCallback;
            _socket.SendAsync(e);
        }
        protected void Send(IList<ArraySegment<byte>> buffers) {
            //_socket.BeginSend(buffers, SocketFlags.None, sendCallback, _socket);

            var e = new SocketAsyncEventArgs();
            e.BufferList = buffers;
            e.Completed += sendCallback;
            _socket.SendAsync(e);
        }


        ///<summary>Stop current action of socket.</summary>
        public void Stop() {
            // 1. dispose
            // 2. why? because we want to maintain State in good logic
            //     Connecting → Aborted → Connecting/Listening
            //     Listening → Aborted → Connecting/Listening
            //     Connected → None → Connecting/Listening
            //   ● When the user calls the next action like Connect or Listen immediately after Stop
            //   ● The acceptCallback, connectCallback event will be callled with error (OperationAborted) Before or After the next action of the user (on another thread)
            //      - If events are call before, we are good
            //      - If events are call after, we will in bad State: Connecting → Aborted → Refused 
            //   => so we can't rely only on those 2 events for handling state

            if (_socket == null) return;

            _socket.Dispose();
            _socket = null;

            // --2--
            if (_state == TcpSocketState.Connecting || _state == TcpSocketState.Listening) 
                State = TcpSocketState.Aborted;
            else if (_state == TcpSocketState.Connected)
                State = TcpSocketState.None;
        }

        void connectCallback(object sender, SocketAsyncEventArgs e) {
            var error = e.SocketError;

            // 2. read void Stop() for more details
            //   ● only change state when sender == _socket 
            switch (error) {
                case SocketError.Success:
                    State = TcpSocketState.Connected;
                    StartRecive(); 
                    break;
                case SocketError.TimedOut:
                    State = TcpSocketState.TimedOut; 
                    break;
                case SocketError.ConnectionRefused:
                    State = TcpSocketState.Refused;
                    break;
                case SocketError.OperationAborted:
                    // --2--
                    if (sender == _socket) {
                        if (_state == TcpSocketState.Connecting) State = TcpSocketState.Aborted;
                    }
                    break;
                default:
                    throw new SocketException((int)error);
            }

        }
        void acceptCallback(object sender, SocketAsyncEventArgs e) {
            var error = e.SocketError;

            // 2. read function Stop() for details
            //   ● only change state when sender == _socket
            switch (error) {
                case SocketError.Success:
                    var newsocket = e.AcceptSocket;
                    OnAccepted(newsocket);
                    _socket?.AcceptAsync(e);
                    break;
                case SocketError.OperationAborted:
                    // --2--
                    if (sender == _socket) {
                        if (_state == TcpSocketState.Listening) State = TcpSocketState.Aborted;
                    }
                    break;
                default:
                    throw new SocketException((int)error);
            }
        }
        void receiveCallback(object sender, SocketAsyncEventArgs e) {
            var error = e.SocketError;
            // 1. Raise
            // 2. disconnected on the correct way - closed, reset or terminated
            // 2. continues reveive    
            // 3. disconnected in other ways:
            //      a. OperationAborted: we disposed us
            //          The overlapped operation was aborted due to the closure of the System.Net.Sockets.Socket.
            //          The I/O operation has been aborted because of either a thread exit or an application request. 
            //      b. ConnectionReset: An existing connection was forcibly closed by the remote host.
            switch (error) {
                case SocketError.Success:
                    var size = e.BytesTransferred;
                    // --1--
                    if (size != 0) OnReceived(_receiveBuffer, 0, size);

                    var r = _socket.Poll(0, SelectMode.SelectRead);
                    if (r == true && _socket.Available == 0) {
                        // --2--
                        _socket.Disconnect(false);
                        State = TcpSocketState.None;
                        return;
                    };
                    // --3--
                    e.SetBuffer(_receiveBuffer, 0, _receiveBuffer.Length);
                    //e.Completed += receiveCallback;
                    _socket.ReceiveAsync(e);
                    break;
                case SocketError.OperationAborted:
                    if (sender == _socket) {
                        if (_state == TcpSocketState.Connected) State = TcpSocketState.None;
                    }
                    break;
                case SocketError.ConnectionReset:
                    State = TcpSocketState.None; break;
                default:
                    throw new SocketException((int)error);
            }
        }
        void sendCallback(object sender, SocketAsyncEventArgs e) {
            var error = e.SocketError;

            switch (error) {
                case SocketError.Success:
                    break;
                case SocketError.OperationAborted:
                    break;
                default:
                    throw new SocketException((int)error);
            }
        }

        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnAccepted(Socket newSocket);
        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnReceived(byte[] buffer, int offset, int size);
        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnStateChanged(TcpSocketState previousState, TcpSocketState state);
        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnDisconnected();
    }
}
//#if !NET6_0_OR_GREATER
//#endif