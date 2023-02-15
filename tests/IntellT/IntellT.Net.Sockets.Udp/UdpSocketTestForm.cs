using Intell.Net.Sockets.Udp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntellT.Net.Sockets.Udp {

    public partial class UdpSocketTestForm: Form {

        List<Tuple<UdpSocketState, UdpSocketState>> listStates = new List<Tuple<UdpSocketState, UdpSocketState>>();
        UdpSocket _socket;


        public UdpSocketTestForm(AddressFamily addressFamily) {
            InitializeComponent();
            _socket = new UdpSocket(addressFamily) {
                StateChanged = onStateChanged,
                Received = onReceived
            };


            _bindButton.Click += (sender, e) => { _socket.Bind(int.Parse(_bindPortText.Text)); };
            _stopButton.Click += (sender, e) => { _socket.Stop(); };
            _disconnectButton.Click += (sender, e) => { _socket?.InternalSocket.Close(); };
            _timerState.Tick += (sender, e) => {
                _stateLabel.Text = _socket.State.ToString();
            };
            _sendToButton.Click += (sender, e) => {
                var endPoint = new IPEndPoint(IPAddress.Parse(_sendToAddressText.Text), int.Parse(_sendToPortText.Text));
                _socket.SendTo(Encoding.ASCII.GetBytes(_sendText.Text), endPoint);
            };


        }

        



        void onStateChanged(object sender, UdpSocketState oldState, UdpSocketState newState) {
            listStates.Add(new Tuple<UdpSocketState, UdpSocketState>(oldState, newState));

            if (this.InvokeRequired == true) this.Invoke(onStateChanged2);
            else onStateChanged2();

        }
        void onStateChanged2() {
            if (this.IsDisposed == true) return;
            for (var i = 0; i < listStates.Count; i++) {
                _logStateText.Text += listStates[i].Item1.ToString() + "→" + listStates[i].Item2.ToString() + "\r\n";
            }
            listStates.Clear();
        }
        void onReceived(UdpSocket sender, byte[] buffer, int offset, int count) {
            this.Invoke(new Action(() => {
                var bytes = new byte[count];
                Buffer.BlockCopy(buffer, offset, bytes, 0, count);
                var text = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                _logText.Text += text;
            }));
        }

    }
}
