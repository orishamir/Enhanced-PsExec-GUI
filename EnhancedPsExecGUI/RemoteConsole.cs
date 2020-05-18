using EnhancedPsExecGUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace EnhancedPsExec
{
    public partial class RemoteConsole : Form
    {
        epsexecForm mainForm;
        Process proc;
        int cmdIndex = 0;
        List<string> lastCmds = new List<string>();
        
        int incByX = 0;
        int incByY = 0;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Console.WriteLine("key: " + (int)keyData);
            
            if (keyData == Keys.Up)
            {
                // -1 because compensating for the fact that im -- first then ..[cmdIndex]
                if (cmdIndex-1 < 0 || lastCmds.Count == 0)
                    return true;

                cmdIndex--;
                updateCmd();
                return true;
            }
            else if (keyData == Keys.Down)
            {
                // +1 because compensating for the fact that im ++ first then ..[cmdIndex]
                if (cmdIndex+1 == lastCmds.Count || lastCmds.Count == 0)
                    return true;

                cmdIndex++;
                updateCmd();
                return true;
            }
            else if(keyData == Keys.Enter){
                sendBtn_Click(new object(), new EventArgs());
                return true;
            }
            else if (keyData == Keys.Tab || keyData == Keys.Escape || keyData == (Keys.Shift | Keys.Tab))
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
        public RemoteConsole(epsexecForm f)
        {
            InitializeComponent();
            
            mainForm = f;
            
            proc = new Process();
            
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            //proc.StartInfo.FileName = "psexec.exe";
            //proc.StartInfo.Arguments = $"\\\\{mainForm.ipBox.Text} -u {mainForm.usrBox.Text} -p {mainForm.passwordBox.Text} -s -accepteula cmd.exe";
            //proc.StartInfo.FileName = "cmd.exe";
            //proc.StartInfo.Arguments = "/c cmd.exe";
            proc.StartInfo.FileName = "paexec.exe";
            proc.StartInfo.Arguments = $"\\\\{mainForm.ipBox.Text} -u {mainForm.usrBox.Text} -p {mainForm.passwordBox.Text} -s -accepteula cmd.exe";

            proc.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
            {
                AppendTextBox(e.Data + "  \n");

            });
            proc.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
            {
                AppendTextBox(e.Data + "");
            });
            
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            //proc.StandardInput.WriteLine("");
        }

        public void AppendTextBox(string value)
        {
           if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            outputBox.AppendText(value);
            outputBox.SelectionStart = outputBox.Text.Length;
            outputBox.ScrollToCaret();
            outputBox.SelectionFont = new Font(FontFamily.GenericSansSerif, (float)fontBox.Value, System.Drawing.FontStyle.Regular);
            
        }

        public void updateCmd()
        {
            Console.WriteLine("idx=" + cmdIndex);
            commandToSendBox.Text = lastCmds[cmdIndex];
            commandToSendBox.SelectionStart = commandToSendBox.Text.Length;
            commandToSendBox.SelectionLength = 0;
        }
        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (commandToSendBox.Text.ToLower().Length > 1)
                lastCmds.Add(commandToSendBox.Text);
            if (commandToSendBox.Text.ToLower() == "cls" || commandToSendBox.Text.ToLower() == "clear")
            {
                clearOutputBoxBtn_Click(new object(), new EventArgs());
                return;
            }
            proc.StandardInput.WriteLine(commandToSendBox.Text);
            Thread.Sleep(800);
            if (commandToSendBox.Text.Length != 0)
                cmdIndex++;
                //proc.StandardInput.WriteLine("");

            commandToSendBox.Clear();
        }

        private void clearOutputBoxBtn_Click(object sender, EventArgs e)
        {
            outputBox.Clear();
            commandToSendBox.Clear();
            //proc.StandardInput.WriteLine("");
        }

        private void outputBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void fontBox_ValueChanged(object sender, EventArgs e)
        {
            outputBox.SelectionFont = new Font(FontFamily.GenericSansSerif, (float)fontBox.Value, System.Drawing.FontStyle.Regular);
        }

        private void RemoteConsole_ResizeBegin(object sender, EventArgs e)
        {
            incByX = this.Width;
            incByY = this.Height;
        }

        private void RemoteConsole_ResizeEnd(object sender, EventArgs e)
        {
            incByX = this.Width - incByX;
            incByY = this.Height - incByY;

            this.outputBox.Width += incByX;
            this.outputBox.Height += incByY;

            this.sendBtn.Location = new Point(this.sendBtn.Location.X + incByX, this.sendBtn.Location.Y + incByY);

            this.commandToSendBox.Location = new Point(this.commandToSendBox.Location.X, this.commandToSendBox.Location.Y + incByY);
            this.commandToSendBox.Width += incByX;

            this.fontBox.Location = new Point(this.fontBox.Location.X + incByX, this.fontBox.Location.Y);

            this.fontSizeLabel.Location = new Point(this.fontSizeLabel.Location.X + incByX, this.fontSizeLabel.Location.Y);
        }

        private void RemoteConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.proc.Close();
        }
    }
}