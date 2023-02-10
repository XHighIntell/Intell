namespace IntellT.Net.Sockets.Udp {
    [TestClassAttribute("Intell.Net.Sockets.Udp.UdpSocket - Ipv6")]
    public class UdpSocketV6 : UdpSocketTestForm {
        public UdpSocketV6() : base(System.Net.Sockets.AddressFamily.InterNetworkV6) {
            _sendToAddressText.Text = "::1";
        }
    }
}
