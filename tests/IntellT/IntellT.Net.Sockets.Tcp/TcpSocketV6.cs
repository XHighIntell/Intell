using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IntellT.Net.Sockets.Tcp {

    [TestClassAttribute("Intell.Net.Sockets.Tcp.TcpSocketV6")]
    public class TcpSocketV6TestForm: TcpSocketTestForm {
        public TcpSocketV6TestForm() : base(AddressFamily.InterNetworkV6) {
            this.Text = "TcpSoket - ipv6";
        }
    }
}
