namespace IntellT.Net.Sockets.Udp {
    [TestClassAttribute("Intell.Net.Sockets.Udp.UdpSocket - Ipv4")]
    public class UdpSocketV4 : UdpSocketTestForm {
        public UdpSocketV4() : base(System.Net.Sockets.AddressFamily.InterNetwork) { }
    }
}
