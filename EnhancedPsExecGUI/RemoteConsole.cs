using EnhancedPsExecGUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace EnhancedPsExec
{
    public partial class RemoteConsole : Form
    {
        epsexecForm mainForm;
        Process proc;
        string lastCmd = "";
        List<string> lastCmds = new List<string>();
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Console.WriteLine("key: " + (int)keyData);
            if (keyData == Keys.Up)
            {
                commandToSendBox.Text = lastCmd;
                return false;
            }

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
            proc.StartInfo.FileName = "cmd.exe";
            //proc.StartInfo.Arguments = $"\\\\{mainForm.ipBox.Text} -u {mainForm.usrBox.Text} -p {mainForm.passwordBox.Text} -s -accepteula cmd.exe";
            proc.StartInfo.Arguments = "/c cmd.exe";
            proc.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
            {
                AppendTextBox("\n" + e.Data);

            });
            proc.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
            {
                AppendTextBox("\n" + e.Data);
            });
            
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            //proc.WaitForExit();
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

        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (commandToSendBox.Text.ToLower() == "cls")
            {
                outputBox.Clear();
                commandToSendBox.Clear();
                return;
            }
            proc.StandardInput.WriteLine(commandToSendBox.Text);
            commandToSendBox.Clear();
        }

        private void clearOutputBoxBtn_Click(object sender, EventArgs e)
        {
            outputBox.Clear();
        }

        private void outputBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void fontBox_ValueChanged(object sender, EventArgs e)
        {
            outputBox.SelectionFont = new Font(FontFamily.GenericSansSerif, (float)fontBox.Value, System.Drawing.FontStyle.Regular);
        }
    }
}