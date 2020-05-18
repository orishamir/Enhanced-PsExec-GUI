namespace EnhancedPsExec
{
    partial class PrefForm
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
            this.components = new System.ComponentModel.Container();
            this.ignoreEmptyIpBox = new System.Windows.Forms.CheckBox();
            this.ignoreNotConnectedBox = new System.Windows.Forms.CheckBox();
            this.alwaysOnTopBox = new System.Windows.Forms.CheckBox();
            this.hideInTaskbarBox = new System.Windows.Forms.CheckBox();
            this.noPaexecBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // ignoreEmptyIpBox
            // 
            this.ignoreEmptyIpBox.AutoSize = true;
            this.ignoreEmptyIpBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ignoreEmptyIpBox.Location = new System.Drawing.Point(38, 24);
            this.ignoreEmptyIpBox.Name = "ignoreEmptyIpBox";
            this.ignoreEmptyIpBox.Size = new System.Drawing.Size(494, 29);
            this.ignoreEmptyIpBox.TabIndex = 0;
            this.ignoreEmptyIpBox.Text = "Ignore the \"IP box not filled\" Error when switching tabs";
            this.ignoreEmptyIpBox.UseVisualStyleBackColor = true;
            this.ignoreEmptyIpBox.Click += new System.EventHandler(this.updateConfig);
            // 
            // ignoreNotConnectedBox
            // 
            this.ignoreNotConnectedBox.AutoSize = true;
            this.ignoreNotConnectedBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ignoreNotConnectedBox.Location = new System.Drawing.Point(38, 100);
            this.ignoreNotConnectedBox.Name = "ignoreNotConnectedBox";
            this.ignoreNotConnectedBox.Size = new System.Drawing.Size(653, 29);
            this.ignoreNotConnectedBox.TabIndex = 1;
            this.ignoreNotConnectedBox.Text = "Ignore the \"You did not connect to the PC\" Warning when switching tabs";
            this.ignoreNotConnectedBox.UseVisualStyleBackColor = true;
            this.ignoreNotConnectedBox.Click += new System.EventHandler(this.updateConfig);
            // 
            // alwaysOnTopBox
            // 
            this.alwaysOnTopBox.AutoSize = true;
            this.alwaysOnTopBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.alwaysOnTopBox.Location = new System.Drawing.Point(38, 177);
            this.alwaysOnTopBox.Name = "alwaysOnTopBox";
            this.alwaysOnTopBox.Size = new System.Drawing.Size(395, 29);
            this.alwaysOnTopBox.TabIndex = 2;
            this.alwaysOnTopBox.Text = "Set the Tool to be the front window always";
            this.alwaysOnTopBox.UseVisualStyleBackColor = true;
            this.alwaysOnTopBox.Click += new System.EventHandler(this.updateConfig);
            // 
            // hideInTaskbarBox
            // 
            this.hideInTaskbarBox.AutoSize = true;
            this.hideInTaskbarBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.hideInTaskbarBox.Location = new System.Drawing.Point(35, 252);
            this.hideInTaskbarBox.Name = "hideInTaskbarBox";
            this.hideInTaskbarBox.Size = new System.Drawing.Size(603, 29);
            this.hideInTaskbarBox.TabIndex = 3;
            this.hideInTaskbarBox.Text = "Hide the Tool in Taskbar making it invisible to an external observer";
            this.hideInTaskbarBox.UseVisualStyleBackColor = true;
            this.hideInTaskbarBox.Click += new System.EventHandler(this.updateConfig);
            // 
            // noPaexecBox
            // 
            this.noPaexecBox.AutoSize = true;
            this.noPaexecBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.noPaexecBox.ForeColor = System.Drawing.Color.Red;
            this.noPaexecBox.Location = new System.Drawing.Point(35, 320);
            this.noPaexecBox.Name = "noPaexecBox";
            this.noPaexecBox.Size = new System.Drawing.Size(795, 29);
            this.noPaexecBox.TabIndex = 4;
            this.noPaexecBox.Text = "Don\'t Use PaExec On the Remote PC (NOT RECOMMENDED, slower but more hidden)";
            this.toolTip1.SetToolTip(this.noPaexecBox, "Some of the features require PaExec.exe to be installed on the REMOTE PC, which c" +
        "an be detected by a sysadmin");
            this.noPaexecBox.UseVisualStyleBackColor = true;
            this.noPaexecBox.Click += new System.EventHandler(this.updateConfig);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 1;
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 1;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 0;
            // 
            // PrefForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 605);
            this.Controls.Add(this.noPaexecBox);
            this.Controls.Add(this.hideInTaskbarBox);
            this.Controls.Add(this.alwaysOnTopBox);
            this.Controls.Add(this.ignoreNotConnectedBox);
            this.Controls.Add(this.ignoreEmptyIpBox);
            this.Name = "PrefForm";
            this.ShowIcon = false;
            this.Text = "Preferences";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox ignoreEmptyIpBox;
        public System.Windows.Forms.CheckBox ignoreNotConnectedBox;
        public System.Windows.Forms.CheckBox alwaysOnTopBox;
        public System.Windows.Forms.CheckBox hideInTaskbarBox;
        public System.Windows.Forms.CheckBox noPaexecBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}