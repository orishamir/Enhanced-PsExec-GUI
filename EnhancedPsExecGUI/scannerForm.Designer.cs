namespace EnhancedPsExec
{
    partial class scannerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(scannerForm));
            this.scanBtn = new System.Windows.Forms.Button();
            this.networkPCsBox = new System.Windows.Forms.ListBox();
            this.getPCSBox = new System.Windows.Forms.Button();
            this.importBtn = new System.Windows.Forms.Button();
            this.selectAllBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // scanBtn
            // 
            this.scanBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.scanBtn.Enabled = false;
            this.scanBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.scanBtn.Location = new System.Drawing.Point(267, 596);
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Size = new System.Drawing.Size(215, 47);
            this.scanBtn.TabIndex = 0;
            this.scanBtn.Text = "Scan Machines Now";
            this.scanBtn.UseVisualStyleBackColor = true;
            this.scanBtn.Click += new System.EventHandler(this.ScanBtn_Click);
            // 
            // networkPCsBox
            // 
            this.networkPCsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.networkPCsBox.FormattingEnabled = true;
            this.networkPCsBox.ItemHeight = 25;
            this.networkPCsBox.Location = new System.Drawing.Point(12, 12);
            this.networkPCsBox.Name = "networkPCsBox";
            this.networkPCsBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.networkPCsBox.Size = new System.Drawing.Size(928, 579);
            this.networkPCsBox.TabIndex = 1;
            this.networkPCsBox.SelectedIndexChanged += new System.EventHandler(this.networkPCsBox_SelectedIndexChanged);
            // 
            // getPCSBox
            // 
            this.getPCSBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.getPCSBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.getPCSBox.Location = new System.Drawing.Point(497, 596);
            this.getPCSBox.Name = "getPCSBox";
            this.getPCSBox.Size = new System.Drawing.Size(214, 47);
            this.getPCSBox.TabIndex = 2;
            this.getPCSBox.Text = "Get Computers List";
            this.getPCSBox.UseVisualStyleBackColor = true;
            this.getPCSBox.Click += new System.EventHandler(this.GetPCSBox_Click);
            // 
            // importBtn
            // 
            this.importBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.importBtn.Enabled = false;
            this.importBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.importBtn.Location = new System.Drawing.Point(87, 596);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(149, 47);
            this.importBtn.TabIndex = 3;
            this.importBtn.Text = "Import";
            this.importBtn.UseVisualStyleBackColor = true;
            this.importBtn.Click += new System.EventHandler(this.ImportBtn_Click);
            // 
            // selectAllBtn
            // 
            this.selectAllBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selectAllBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.selectAllBtn.Location = new System.Drawing.Point(946, 12);
            this.selectAllBtn.Name = "selectAllBtn";
            this.selectAllBtn.Size = new System.Drawing.Size(125, 37);
            this.selectAllBtn.TabIndex = 4;
            this.selectAllBtn.Text = "Select All";
            this.selectAllBtn.UseVisualStyleBackColor = true;
            this.selectAllBtn.Click += new System.EventHandler(this.SelectAllBtn_Click);
            // 
            // scannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 674);
            this.Controls.Add(this.selectAllBtn);
            this.Controls.Add(this.importBtn);
            this.Controls.Add(this.getPCSBox);
            this.Controls.Add(this.networkPCsBox);
            this.Controls.Add(this.scanBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "scannerForm";
            this.ShowInTaskbar = false;
            this.Text = "scanner";
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.ListBox networkPCsBox;
        public System.Windows.Forms.Button scanBtn;
        public System.Windows.Forms.Button getPCSBox;
        public System.Windows.Forms.Button importBtn;
        private System.Windows.Forms.Button selectAllBtn;
    }
}