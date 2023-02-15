namespace IntellT.Net.Sockets.Tcp {
    [TestClassAttribute("Intell.Net.Sockets.Tcp.TcpSocket - Ipv4")]
    public class TcpSocketV4TestForm: TcpSocketTestForm {
        public TcpSocketV4TestForm() : base(System.Net.Sockets.AddressFamily.InterNetworkV6) { 
        
        }
    }
}
