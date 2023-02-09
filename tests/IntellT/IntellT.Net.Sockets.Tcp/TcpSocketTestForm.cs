using Intell.Net.Sockets.Tcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace IntellT.Net.Sockets.Tcp {


    public partial class TcpSocketTestForm: Form {

        AddressFamily _addressFamily;
        TcpSocket _socket;
        List<Tuple<TcpSocketState, TcpSocketState>> listStates = new List<Tuple<TcpSocketState, TcpSocketState>>();
        
        public TcpSocketTestForm(AddressFamily addressFamily) {
            InitializeComponent();

            _addressFamily = addressFamily;
            _socket = new TcpSocket(_addressFamily);

            // normal tests
            this._listenButton.Click += (sender, e) => {
                //var endpoint = IPEndPoint.Parse("0.0.0.0");
                //endpoint.Port = int.Parse(_portText.Text);
                _socket.Accepted = onAccepted;
                _socket.StateChanged = onStateChanged;
                //_socket.Listen(endpoint);
                _socket.Listen(int.Parse(_portText.Text));
            };
            this._connectButton.Click += (sender, e) => {
                var endpoint = new DnsEndPoint(_addressText.Text, int.Parse(_portText.Text), _addressFamily);

                _socket.Received = onReceived;
                _socket.StateChanged = onStateChanged;
                _socket.Connect(endpoint);
            };
            this._stopButton.Click += (sender, e) => { _socket.Stop(); };
            this._sendButton.Click += (sender, e) => {
                var bytes = Encoding.ASCII.GetBytes(_sendText.Text);
                _socket.Send(bytes);
            };
            this._sendButton2.Click += (sender, e) => {
                var bytes1 = Encoding.ASCII.GetBytes("DATA1");
                var bytes2 = Encoding.ASCII.GetBytes("DATA2");

                var x = new ArraySegment<byte>[] { bytes1, bytes2 };
                _socket.Send(x);
            };
            // other tests
            this._shutdownButton.Click += (sender, e) => {
                _socket.InternalSocket?.Shutdown(SocketShutdown.Both);
            };
            this._disconnectButton.Click += (sender, e) => {
                _socket.InternalSocket?.Disconnect(false);
            };
            this._closeButton.Click += (sender, e) => {
                _socket.InternalSocket?.Close();
                _socket.InternalSocket?.Dispose();
            };

            this.Load += (sender, e) => { };
            this.FormClosing += (sender, e) => { _socket?.Stop(); };

        }



        void onReceived(TcpSocket sender, byte[] buffer, int offset, int count) {
            this.Invoke(new Action(() => {
                var bytes = new byte[count];
                Buffer.BlockCopy(buffer, offset, bytes, 0, count);
                var text = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                _logText.Text += text;
            }));
        }
        void onAccepted(TcpSocket sender, Socket newSocket) {
            _socket.Stop();
            _socket = new TcpSocket(newSocket);
            _socket.Received = onReceived;
            _socket.StateChanged = onStateChanged;
            //_socket.StartRecive();
        }
        void onStateChanged(object sender, TcpSocketState oldState, TcpSocketState newState) {
            listStates.Add(new Tuple<TcpSocketState, TcpSocketState>(oldState, newState));

            if (this.InvokeRequired == true) this.Invoke(onStateChanged2);
            else onStateChanged2();
            
        }
        void onStateChanged2() {
            if (this.IsDisposed == true) return;
            for (var i = 0; i < listStates.Count; i++) {
                textBox1.Text += listStates[i].Item1.ToString() + "→" + listStates[i].Item2.ToString() + "\r\n";
            }
            listStates.Clear();
        }

        

        private void timer1_Tick(object sender, EventArgs e) {
            if (_socket == null) return;
            _stateLabel.Text = _socket?.State.ToString();
        }


    }
}
