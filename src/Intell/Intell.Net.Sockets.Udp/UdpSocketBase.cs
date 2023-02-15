using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace Intell.Net.Sockets.Udp {

    ///<summary>Implements generic UPD Socket for both IPv4 and IPv6.</summary>
    public abstract class UdpSocketBase {

        Socket _socket;
        UdpSocketState _state = UdpSocketState.None;
        readonly AddressFamily _addressFamily;
        byte[] _receiveBuffer; // that buffer is used for receiving 


        ///<summary>Initializes a new instance of the <seealso cref="UdpSocketBase"/> class.</summary>
        public UdpSocketBase(AddressFamily addressFamily) { _addressFamily = addressFamily; }


        #region properties
        ///<summary>Gets internal <see cref="Socket"/>.</summary>
        public Socket InternalSocket { get { return _socket; } }
        public UdpSocketState State {
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


        public void Bind(int port) {
            if (_addressFamily == AddressFamily.InterNetwork) Bind(new IPEndPoint(0, port));
            else if (_addressFamily == AddressFamily.InterNetworkV6) Bind(new IPEndPoint(IPAddress.IPv6Any, port));
            else throw new NotSupportedException("AddressFamily is not supported");
        }
        public void Bind(EndPoint endPoint) {
            // 1. only stop if we already in Bind
            if (_state == UdpSocketState.Bind) Stop();
            if (_receiveBuffer == null) _receiveBuffer = new byte[8192];

            _socket = new Socket(_addressFamily, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(endPoint);
            State = UdpSocketState.Bind;

            var e = new SocketAsyncEventArgs();
            e.SetBuffer(_receiveBuffer, 0, _receiveBuffer.Length);
            e.Completed += receiveCallback;
            e.RemoteEndPoint = endPoint;
            _socket.ReceiveFromAsync(e);
        }
        public void Stop() {
            if (_socket == null) return;

            _socket.Dispose();
            _socket = null;

            // --2--
            State = UdpSocketState.None;
        }

        protected void SendTo(byte[] buffer, IPEndPoint endPoint) { SendTo(buffer, 0, buffer.Length, endPoint); }
        protected void SendTo(byte[] buffer, int offset, int count, IPEndPoint endPoint) {
            if (_socket == null) {
                _socket = new Socket(_addressFamily, SocketType.Dgram, ProtocolType.Udp);
                State = UdpSocketState.Ready;
            }
            

            var e = new SocketAsyncEventArgs();
            e.SetBuffer(buffer, offset, count);
            e.RemoteEndPoint = endPoint;
            e.Completed += sendCallback;
            _socket.SendToAsync(e);
        }
        protected void SendTo(IList<ArraySegment<byte>> buffers, IPEndPoint endPoint) {
            if (_socket == null) {
                _socket = new Socket(_addressFamily, SocketType.Dgram, ProtocolType.Udp);
                State = UdpSocketState.Ready;
            }

            var e = new SocketAsyncEventArgs();
            e.BufferList = buffers;
            e.Completed += sendCallback;
            e.RemoteEndPoint = endPoint;
              
            _socket.SendAsync(e);
        }

        void receiveCallback(object sender, SocketAsyncEventArgs e) {
            var error = e.SocketError;
            switch (error) {
                case SocketError.Success:
                    var size = e.BytesTransferred;
                    // --1--
                    if (size != 0) OnReceived(_receiveBuffer, 0, size);

                    var r = _socket.Poll(0, SelectMode.SelectRead);
                    if (r == true && _socket.Available == 0) {
                        // --2--
                        State = UdpSocketState.None;
                        return;
                    };
                    // --3--
                    e.SetBuffer(_receiveBuffer, 0, _receiveBuffer.Length);
                    _socket.ReceiveFromAsync(e);
                    break;
                case SocketError.OperationAborted:
                    if (sender == _socket) {
                        if (_state == UdpSocketState.Bind) State = UdpSocketState.None;
                    }
                    break;
                default:
                    throw new SocketException((int)error);
            }

            var x = e;
        }
        void sendCallback(object sender, SocketAsyncEventArgs e) {
            var error = e.SocketError;

            switch (error) {
                case SocketError.Success:
                    break;
                case SocketError.OperationAborted:
                    if (sender == _socket) {
                        if (_state == UdpSocketState.Ready) State = UdpSocketState.None;
                    }
                    break;
                default:
                    throw new SocketException((int)error);
            }
        }


        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnReceived(byte[] buffer, int offset, int size);
        ///<summary>A empty callback override it to customize.</summary>
        protected abstract void OnStateChanged(UdpSocketState previousState, UdpSocketState state);
    }
}
