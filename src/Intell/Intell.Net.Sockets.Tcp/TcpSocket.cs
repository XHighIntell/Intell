using System;
using System.Collections.Generic;
using System.Net.Sockets;


namespace Intell.Net.Sockets.Tcp {

    public delegate void TcpSocketReceiveHandler(TcpSocket socket, byte[] buffer, int offset, int count);
    public delegate void TcpSocketStateChangeHandler(TcpSocket socket, TcpSocketState oldState, TcpSocketState newState);
    public delegate void TcpSocketAccepteHandler(TcpSocket sender, Socket newSocket);
    public delegate void TcpSocketDisconnectedHandler(TcpSocket sender);

    /// <summary>
    /// The <see cref="TcpSocket"/> class provide TCP services at a higher level
    /// of abstraction than the <see cref="Socket"/>. Better asynchronous programming using callback events.
    /// </summary>
    public class TcpSocket: TcpSocketBase {

        ///<summary>Initializes a new instance of the <see cref="TcpSocket"/> class.</summary>
        public TcpSocket(AddressFamily addressFamily) : base(addressFamily) { }

        ///<summary>Initializes a new instance of the <see cref="TcpSocket"/> class.</summary>
        public TcpSocket(Socket socket) : base(socket) {}

        ///<summary>Occurs when a new <see cref="Socket"/> is created for incoming connection.</summary>
        public TcpSocketAccepteHandler Accepted { get; set; }

        ///<summary>Occurs when data received.</summary>
        public TcpSocketReceiveHandler Received { get; set; }

        ///<summary>Occurs when the <see cref="TcpSocketState"/> changes.</summary>
        public TcpSocketStateChangeHandler StateChanged { get; set; }

        ///<summary>Occurs when <see cref="Socket"/> disconnects.</summary>
        public TcpSocketDisconnectedHandler Disconnected { get; set; }

        ///<summary>Send data to connected <see cref="Socket"/>.</summary>
        public new void Send(byte[] buffer) { base.Send(buffer); }
        ///<summary>Send data to connected <see cref="Socket"/>.</summary>
        public new void Send(byte[] buffer, int offset, int count) { base.Send(buffer, offset, count); }
        ///<summary>Send data to connected <see cref="Socket"/>.</summary>
        public new void Send(IList<ArraySegment<byte>> buffers) { base.Send(buffers); }


        protected override void OnAccepted(Socket newSocket) {
            Accepted?.Invoke(this, newSocket);
        }
        protected override void OnReceived(byte[] buffer, int offset, int size) {
            Received?.Invoke(this, buffer, offset, size);
        }
        protected override void OnStateChanged(TcpSocketState oldState, TcpSocketState newState) { StateChanged?.Invoke(this, oldState, newState); }
        protected override void OnDisconnected() { Disconnected?.Invoke(this); }
    }
}
