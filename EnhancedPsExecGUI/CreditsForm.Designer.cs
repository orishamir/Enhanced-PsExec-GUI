namespace EnhancedPsExec
{
    partial class CreditsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreditsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.ytStart = new System.Windows.Forms.Button();
            this.githubBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.focusThis = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(31, 0, 31, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(366, 84);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enhanced PsExec was made by Ori Shamir.";
            // 
            // ytStart
            // 
            this.ytStart.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ytStart.BackgroundImage")));
            this.ytStart.Cursor = System.Windows.Forms.Cursors.Help;
            this.ytStart.Location = new System.Drawing.Point(572, 361);
            this.ytStart.Name = "ytStart";
            this.ytStart.Size = new System.Drawing.Size(131, 106);
            this.ytStart.TabIndex = 1;
            this.toolTip1.SetToolTip(this.ytStart, "Visit My YouTube Channel");
            this.ytStart.UseVisualStyleBackColor = true;
            this.ytStart.Click += new System.EventHandler(this.YtStart_Click);
            // 
            // githubBtn
            // 
            this.githubBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("githubBtn.BackgroundImage")));
            this.githubBtn.Cursor = System.Windows.Forms.Cursors.Help;
            this.githubBtn.Location = new System.Drawing.Point(12, 361);
            this.githubBtn.Name = "githubBtn";
            this.githubBtn.Size = new System.Drawing.Size(110, 106);
            this.githubBtn.TabIndex = 2;
            this.toolTip1.SetToolTip(this.githubBtn, "Visit My GitHub Page");
            this.githubBtn.UseVisualStyleBackColor = true;
            this.githubBtn.Click += new System.EventHandler(this.GithubBtn_Click);
            // 
            // label2
            // 
            this.label2.Cursor = System.Windows.Forms.Cursors.Help;
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.Location = new System.Drawing.Point(298, 361);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 109);
            this.label2.TabIndex = 3;
            this.toolTip1.SetToolTip(this.label2, "Visit This Project On PyPi");
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // focusThis
            // 
            this.focusThis.Location = new System.Drawing.Point(650, 9);
            this.focusThis.Name = "focusThis";
            this.focusThis.Size = new System.Drawing.Size(0, 0);
            this.focusThis.TabIndex = 4;
            this.focusThis.Text = "button1";
            this.focusThis.UseVisualStyleBackColor = true;
            // 
            // CreditsForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(31F, 61F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(715, 479);
            this.Controls.Add(this.focusThis);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.githubBtn);
            this.Controls.Add(this.ytStart);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Margin = new System.Windows.Forms.Padding(31, 0, 31, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreditsForm";
            this.Opacity = 0.92D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Credits";
            this.Load += new System.EventHandler(this.CreditsFormBox_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.focusIt);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.focusIt);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.focusIt);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.focusIt);
            this.MouseHover += new System.EventHandler(this.focusIt);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.focusIt);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ytStart;
        private System.Windows.Forms.Button githubBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button focusThis;
    }
}