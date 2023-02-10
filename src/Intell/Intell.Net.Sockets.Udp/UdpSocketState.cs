/*
    None: Socket is not created yet
    Ready: Socket is created
    Bind: Socket is binded to a local port
  
    None → Ready
    None → Bind
    Ready → Bind
    Ready → None
    Bind → None
*/


namespace Intell.Net.Sockets.Udp {
    ///<summary>Specifies values representing possible states for an <see cref="UdpSocketBase"/> object.</summary>
    public enum UdpSocketState {
        None = 0,
        Ready = 1,
        Bind = 2,
    }
}