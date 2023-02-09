namespace IntellT {
    partial class EntryForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this._objectBrowserTreeView = new System.Windows.Forms.TreeView();
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // _objectBrowserTreeView
            // 
            this._objectBrowserTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._objectBrowserTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._objectBrowserTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._objectBrowserTreeView.ImageIndex = 0;
            this._objectBrowserTreeView.ImageList = this._imageList;
            this._objectBrowserTreeView.Indent = 23;
            this._objectBrowserTreeView.ItemHeight = 30;
            this._objectBrowserTreeView.Location = new System.Drawing.Point(0, 0);
            this._objectBrowserTreeView.Name = "_objectBrowserTreeView";
            this._objectBrowserTreeView.SelectedImageIndex = 0;
            this._objectBrowserTreeView.ShowLines = false;
            this._objectBrowserTreeView.Size = new System.Drawing.Size(424, 606);
            this._objectBrowserTreeView.TabIndex = 0;
            // 
            // _imageList
            // 
            this._imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this._imageList.ImageSize = new System.Drawing.Size(20, 20);
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // EntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 606);
            this.Controls.Add(this._objectBrowserTreeView);
            this.Name = "EntryForm";
            this.ShowIcon = false;
            this.Text = "Double click to choose what to debug";
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView _objectBrowserTreeView;
        private ImageList _imageList;
    }
}