using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IntellT.Net.Sockets.Tcp {

    [TestClassAttribute("Intell.Net.Sockets.Tcp.TcpSocketV4")]
    public class TcpSocketV4TestForm: TcpSocketTestForm {
        public TcpSocketV4TestForm() : base(AddressFamily.InterNetworkV6) { 
        
        }
    }
}
