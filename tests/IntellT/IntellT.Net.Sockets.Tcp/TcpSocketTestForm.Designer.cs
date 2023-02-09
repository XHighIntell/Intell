namespace IntellT.Net.Sockets.Tcp {
    partial class TcpSocketTestForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this._listenButton = new System.Windows.Forms.Button();
            this._stopButton = new System.Windows.Forms.Button();
            this._connectButton = new System.Windows.Forms.Button();
            this._logText = new System.Windows.Forms.TextBox();
            this._sendButton = new System.Windows.Forms.Button();
            this._sendText = new System.Windows.Forms.TextBox();
            this._portText = new System.Windows.Forms.TextBox();
            this._addressText = new System.Windows.Forms.TextBox();
            this._shutdownButton = new System.Windows.Forms.Button();
            this._stateLabel = new System.Windows.Forms.Label();
            this._disconnectButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this._sendButton2 = new System.Windows.Forms.Button();
            this._stateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _listenButton
            // 
            this._listenButton.Location = new System.Drawing.Point(332, 7);
            this._listenButton.Name = "_listenButton";
            this._listenButton.Size = new System.Drawing.Size(94, 29);
            this._listenButton.TabIndex = 0;
            this._listenButton.Text = "Listen";
            this._listenButton.UseVisualStyleBackColor = true;
            // 
            // _stopButton
            // 
            this._stopButton.Location = new System.Drawing.Point(432, 7);
            this._stopButton.Name = "_stopButton";
            this._stopButton.Size = new System.Drawing.Size(94, 29);
            this._stopButton.TabIndex = 1;
            this._stopButton.Text = "Stop";
            this._stopButton.UseVisualStyleBackColor = true;
            // 
            // _connectButton
            // 
            this._connectButton.Location = new System.Drawing.Point(228, 7);
            this._connectButton.Name = "_connectButton";
            this._connectButton.Size = new System.Drawing.Size(98, 29);
            this._connectButton.TabIndex = 2;
            this._connectButton.Text = "Connect";
            this._connectButton.UseVisualStyleBackColor = true;
            // 
            // _logText
            // 
            this._logText.Location = new System.Drawing.Point(5, 44);
            this._logText.Multiline = true;
            this._logText.Name = "_logText";
            this._logText.Size = new System.Drawing.Size(421, 281);
            this._logText.TabIndex = 3;
            // 
            // _sendButton
            // 
            this._sendButton.Location = new System.Drawing.Point(432, 383);
            this._sendButton.Name = "_sendButton";
            this._sendButton.Size = new System.Drawing.Size(94, 55);
            this._sendButton.TabIndex = 4;
            this._sendButton.Text = "Send";
            this._sendButton.UseVisualStyleBackColor = true;
            // 
            // _sendText
            // 
            this._sendText.Location = new System.Drawing.Point(5, 331);
            this._sendText.Multiline = true;
            this._sendText.Name = "_sendText";
            this._sendText.Size = new System.Drawing.Size(421, 107);
            this._sendText.TabIndex = 3;
            // 
            // _portText
            // 
            this._portText.Location = new System.Drawing.Point(136, 9);
            this._portText.Name = "_portText";
            this._portText.Size = new System.Drawing.Size(86, 27);
            this._portText.TabIndex = 5;
            this._portText.Text = "99";
            this._portText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _addressText
            // 
            this._addressText.Location = new System.Drawing.Point(5, 9);
            this._addressText.Name = "_addressText";
            this._addressText.Size = new System.Drawing.Size(125, 27);
            this._addressText.TabIndex = 5;
            this._addressText.Text = "localhost";
            this._addressText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _shutdownButton
            // 
            this._shutdownButton.Location = new System.Drawing.Point(432, 303);
            this._shutdownButton.Name = "_shutdownButton";
            this._shutdownButton.Size = new System.Drawing.Size(104, 29);
            this._shutdownButton.TabIndex = 1;
            this._shutdownButton.Text = "Shutdown";
            this._shutdownButton.UseVisualStyleBackColor = true;
            // 
            // _stateLabel
            // 
            this._stateLabel.AutoSize = true;
            this._stateLabel.Location = new System.Drawing.Point(532, 11);
            this._stateLabel.Name = "_stateLabel";
            this._stateLabel.Size = new System.Drawing.Size(50, 20);
            this._stateLabel.TabIndex = 6;
            this._stateLabel.Text = "NONE";
            // 
            // _disconnectButton
            // 
            this._disconnectButton.Location = new System.Drawing.Point(542, 303);
            this._disconnectButton.Name = "_disconnectButton";
            this._disconnectButton.Size = new System.Drawing.Size(104, 29);
            this._disconnectButton.TabIndex = 7;
            this._disconnectButton.Text = "Disconnect";
            this._disconnectButton.UseVisualStyleBackColor = true;
            // 
            // _closeButton
            // 
            this._closeButton.Location = new System.Drawing.Point(542, 268);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(104, 29);
            this._closeButton.TabIndex = 8;
            this._closeButton.Text = "Close";
            this._closeButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(432, 272);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Internal Socket";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(432, 44);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(214, 218);
            this.textBox1.TabIndex = 10;
            // 
            // _sendButton2
            // 
            this._sendButton2.Location = new System.Drawing.Point(532, 383);
            this._sendButton2.Name = "_sendButton2";
            this._sendButton2.Size = new System.Drawing.Size(114, 55);
            this._sendButton2.TabIndex = 11;
            this._sendButton2.Text = "Send multi buffers";
            this._sendButton2.UseVisualStyleBackColor = true;
            // 
            // _stateTimer
            // 
            this._stateTimer.Enabled = true;
            this._stateTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // UltraSocketTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 442);
            this.Controls.Add(this._sendButton2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._disconnectButton);
            this.Controls.Add(this._stateLabel);
            this.Controls.Add(this._addressText);
            this.Controls.Add(this._portText);
            this.Controls.Add(this._sendButton);
            this.Controls.Add(this._sendText);
            this.Controls.Add(this._logText);
            this.Controls.Add(this._connectButton);
            this.Controls.Add(this._shutdownButton);
            this.Controls.Add(this._stopButton);
            this.Controls.Add(this._listenButton);
            this.Name = "UltraSocketTestForm";
            this.Text = "TcpSocket";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button _listenButton;
        private Button _stopButton;
        private Button _connectButton;
        private TextBox _logText;
        private Button _sendButton;
        private TextBox _sendText;
        private TextBox _portText;
        private TextBox _addressText;
        private Button _shutdownButton;
        private Button _disconnectButton;
        private Button _closeButton;
        private Label label2;
        private TextBox textBox1;
        private Button _sendButton2;
        private System.Windows.Forms.Timer _stateTimer;
        private Label _stateLabel;
    }
}