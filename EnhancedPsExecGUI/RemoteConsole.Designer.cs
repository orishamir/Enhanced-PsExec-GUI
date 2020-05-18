namespace EnhancedPsExec
{
    partial class RemoteConsole
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.commandToSendBox = new System.Windows.Forms.TextBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.clearOutputBoxBtn = new System.Windows.Forms.Button();
            this.fontBox = new System.Windows.Forms.NumericUpDown();
            this.fontSizeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fontBox)).BeginInit();
            this.SuspendLayout();
            // 
            // commandToSendBox
            // 
            this.commandToSendBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.commandToSendBox.Location = new System.Drawing.Point(15, 575);
            this.commandToSendBox.Name = "commandToSendBox";
            this.commandToSendBox.Size = new System.Drawing.Size(862, 30);
            this.commandToSendBox.TabIndex = 1;
            // 
            // sendBtn
            // 
            this.sendBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.sendBtn.Location = new System.Drawing.Point(883, 572);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(133, 33);
            this.sendBtn.TabIndex = 2;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // outputBox
            // 
            this.outputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.outputBox.Location = new System.Drawing.Point(15, 44);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(862, 525);
            this.outputBox.TabIndex = 4;
            this.outputBox.Text = "";
            this.outputBox.WordWrap = false;
            this.outputBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.outputBox_LinkClicked);
            // 
            // clearOutputBoxBtn
            // 
            this.clearOutputBoxBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.clearOutputBoxBtn.Location = new System.Drawing.Point(15, 5);
            this.clearOutputBoxBtn.Name = "clearOutputBoxBtn";
            this.clearOutputBoxBtn.Size = new System.Drawing.Size(133, 33);
            this.clearOutputBoxBtn.TabIndex = 5;
            this.clearOutputBoxBtn.Text = "Clear";
            this.clearOutputBoxBtn.UseVisualStyleBackColor = true;
            this.clearOutputBoxBtn.Click += new System.EventHandler(this.clearOutputBoxBtn_Click);
            // 
            // fontBox
            // 
            this.fontBox.DecimalPlaces = 2;
            this.fontBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.fontBox.Location = new System.Drawing.Point(888, 72);
            this.fontBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fontBox.Name = "fontBox";
            this.fontBox.Size = new System.Drawing.Size(96, 30);
            this.fontBox.TabIndex = 6;
            this.fontBox.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.fontBox.ValueChanged += new System.EventHandler(this.fontBox_ValueChanged);
            // 
            // fontSizeLabel
            // 
            this.fontSizeLabel.AutoSize = true;
            this.fontSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.fontSizeLabel.Location = new System.Drawing.Point(883, 44);
            this.fontSizeLabel.Name = "fontSizeLabel";
            this.fontSizeLabel.Size = new System.Drawing.Size(101, 25);
            this.fontSizeLabel.TabIndex = 7;
            this.fontSizeLabel.Text = "Font Size:";
            // 
            // RemoteConsole
            // 
            this.AcceptButton = this.sendBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 617);
            this.Controls.Add(this.fontSizeLabel);
            this.Controls.Add(this.fontBox);
            this.Controls.Add(this.clearOutputBoxBtn);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.commandToSendBox);
            this.Name = "RemoteConsole";
            this.ShowIcon = false;
            this.Text = "RemoteConsole";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteConsole_FormClosing);
            this.ResizeBegin += new System.EventHandler(this.RemoteConsole_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.RemoteConsole_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.fontBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox commandToSendBox;
        public System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.RichTextBox outputBox;
        public System.Windows.Forms.Button clearOutputBoxBtn;
        private System.Windows.Forms.NumericUpDown fontBox;
        private System.Windows.Forms.Label fontSizeLabel;
    }
}