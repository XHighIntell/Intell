namespace Intell.Net.Sockets.Tcp {
    ///<summary>Specifies values representing possible states for an <see cref="TcpSocketBase"/> object.</summary>
    public enum TcpSocketState {
        None,
        Connecting,
        Connected,
        ///<summary>No connection could be made because the target machine actively refused it</summary>
        Refused,
        TimedOut,
        Aborted,
        Listening,
        Error
    }
    public enum SocketNextAction { Connect, Listen }
}

// Refused