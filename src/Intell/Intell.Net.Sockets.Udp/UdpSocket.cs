using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Intell.Net.Sockets.Udp {

    public delegate void UdpSocketReceiveHandler(UdpSocket socket, byte[] buffer, int offset, int count);
    public delegate void UdpSocketStateChangeHandler(UdpSocket socket, UdpSocketState oldState, UdpSocketState newState);


    public class UdpSocket: UdpSocketBase {

        public UdpSocket(AddressFamily addressFamily) : base(addressFamily) {}

        ///<summary>Occurs when data received.</summary>
        public UdpSocketReceiveHandler Received { get; set; }

        ///<summary>Occurs when the <see cref="UdpSocketState"/> changes.</summary>
        public UdpSocketStateChangeHandler StateChanged { get; set; }


        public new void SendTo(byte[] buffer, IPEndPoint endPoint) { base.SendTo(buffer, 0, buffer.Length, endPoint); }
        public new void SendTo(byte[] buffer, int offset, int count, IPEndPoint endPoint) { base.SendTo(buffer, offset, count, endPoint); }
        public new void SendTo(IList<ArraySegment<byte>> buffers, IPEndPoint endPoint) { base.SendTo(buffers, endPoint); }

        protected override void OnReceived(byte[] buffer, int offset, int count) {
            Received?.Invoke(this, buffer, offset, count);
        }
        protected override void OnStateChanged(UdpSocketState oldState, UdpSocketState newstate) {
            StateChanged?.Invoke(this, oldState, newstate);
        }
    }
}
