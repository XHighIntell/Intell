namespace IntellT.Net.Sockets.Tcp {
    [TestClassAttribute("Intell.Net.Sockets.Tcp.TcpSocket - Ipv6")]
    public class TcpSocketV6TestForm: TcpSocketTestForm {
        public TcpSocketV6TestForm() : base(System.Net.Sockets.AddressFamily.InterNetworkV6) {
            this.Text = "TcpSoket - ipv6";
        }
    }
}
