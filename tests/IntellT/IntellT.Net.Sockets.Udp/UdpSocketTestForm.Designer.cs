namespace IntellT.Net.Sockets.Udp {
    partial class UdpSocketTestForm {
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
            this._sendToButton = new System.Windows.Forms.Button();
            this._bindButton = new System.Windows.Forms.Button();
            this._stopButton = new System.Windows.Forms.Button();
            this._disconnectButton = new System.Windows.Forms.Button();
            this._stateLabel = new System.Windows.Forms.Label();
            this._logStateText = new System.Windows.Forms.TextBox();
            this._timerState = new System.Windows.Forms.Timer(this.components);
            this._sendText = new System.Windows.Forms.TextBox();
            this._sendToAddressText = new System.Windows.Forms.TextBox();
            this._bindPortText = new System.Windows.Forms.TextBox();
            this._sendToPortText = new System.Windows.Forms.TextBox();
            this._logText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _sendToButton
            // 
            this._sendToButton.Location = new System.Drawing.Point(369, 366);
            this._sendToButton.Name = "_sendToButton";
            this._sendToButton.Size = new System.Drawing.Size(217, 40);
            this._sendToButton.TabIndex = 0;
            this._sendToButton.Text = "SendTo";
            this._sendToButton.UseVisualStyleBackColor = true;
            // 
            // _bindButton
            // 
            this._bindButton.Location = new System.Drawing.Point(104, 12);
            this._bindButton.Name = "_bindButton";
            this._bindButton.Size = new System.Drawing.Size(94, 30);
            this._bindButton.TabIndex = 1;
            this._bindButton.Text = "Bind";
            this._bindButton.UseVisualStyleBackColor = true;
            // 
            // _stopButton
            // 
            this._stopButton.Location = new System.Drawing.Point(204, 12);
            this._stopButton.Name = "_stopButton";
            this._stopButton.Size = new System.Drawing.Size(94, 30);
            this._stopButton.TabIndex = 2;
            this._stopButton.Text = "Stop";
            this._stopButton.UseVisualStyleBackColor = true;
            // 
            // _disconnectButton
            // 
            this._disconnectButton.Location = new System.Drawing.Point(383, 13);
            this._disconnectButton.Name = "_disconnectButton";
            this._disconnectButton.Size = new System.Drawing.Size(94, 29);
            this._disconnectButton.TabIndex = 3;
            this._disconnectButton.Text = "Disconnect";
            this._disconnectButton.UseVisualStyleBackColor = true;
            // 
            // _stateLabel
            // 
            this._stateLabel.AutoSize = true;
            this._stateLabel.Location = new System.Drawing.Point(304, 17);
            this._stateLabel.Name = "_stateLabel";
            this._stateLabel.Size = new System.Drawing.Size(45, 20);
            this._stateLabel.TabIndex = 4;
            this._stateLabel.Text = "None";
            // 
            // _logStateText
            // 
            this._logStateText.Location = new System.Drawing.Point(369, 48);
            this._logStateText.Multiline = true;
            this._logStateText.Name = "_logStateText";
            this._logStateText.Size = new System.Drawing.Size(217, 238);
            this._logStateText.TabIndex = 5;
            // 
            // _timerState
            // 
            this._timerState.Enabled = true;
            // 
            // _sendText
            // 
            this._sendText.Location = new System.Drawing.Point(12, 288);
            this._sendText.Multiline = true;
            this._sendText.Name = "_sendText";
            this._sendText.Size = new System.Drawing.Size(351, 150);
            this._sendText.TabIndex = 6;
            this._sendText.Text = "Hello";
            // 
            // _sendToAddressText
            // 
            this._sendToAddressText.Location = new System.Drawing.Point(369, 412);
            this._sendToAddressText.Name = "_sendToAddressText";
            this._sendToAddressText.Size = new System.Drawing.Size(125, 27);
            this._sendToAddressText.TabIndex = 8;
            this._sendToAddressText.Text = "127.0.0.1";
            this._sendToAddressText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _bindPortText
            // 
            this._bindPortText.Location = new System.Drawing.Point(12, 14);
            this._bindPortText.Name = "_bindPortText";
            this._bindPortText.Size = new System.Drawing.Size(86, 27);
            this._bindPortText.TabIndex = 9;
            this._bindPortText.Text = "99";
            this._bindPortText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _sendToPortText
            // 
            this._sendToPortText.Location = new System.Drawing.Point(500, 412);
            this._sendToPortText.Name = "_sendToPortText";
            this._sendToPortText.Size = new System.Drawing.Size(86, 27);
            this._sendToPortText.TabIndex = 10;
            this._sendToPortText.Text = "99";
            this._sendToPortText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _logText
            // 
            this._logText.Location = new System.Drawing.Point(12, 48);
            this._logText.Multiline = true;
            this._logText.Name = "_logText";
            this._logText.Size = new System.Drawing.Size(351, 234);
            this._logText.TabIndex = 11;
            // 
            // UdpSocketTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 450);
            this.Controls.Add(this._logText);
            this.Controls.Add(this._sendToPortText);
            this.Controls.Add(this._sendToAddressText);
            this.Controls.Add(this._bindPortText);
            this.Controls.Add(this._sendText);
            this.Controls.Add(this._logStateText);
            this.Controls.Add(this._stateLabel);
            this.Controls.Add(this._disconnectButton);
            this.Controls.Add(this._stopButton);
            this.Controls.Add(this._bindButton);
            this.Controls.Add(this._sendToButton);
            this.Name = "UdpSocketTestForm";
            this.Text = "UdpSocket";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button _sendToButton;
        private Button _bindButton;
        private Button _stopButton;
        private Button _disconnectButton;
        private Label _stateLabel;
        private TextBox _logStateText;
        private System.Windows.Forms.Timer _timerState;
        private TextBox _sendText;
        protected TextBox _sendToAddressText;
        private TextBox _bindPortText;
        private TextBox _sendToPortText;
        private TextBox _logText;
    }
}