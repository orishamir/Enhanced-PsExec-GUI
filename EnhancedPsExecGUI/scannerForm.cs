using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Net.Sockets;
using EnhancedPsExecGUI;
using System.Diagnostics;

namespace EnhancedPsExec
{
    public partial class scannerForm : Form
    {
        epsexecForm mainForm;
        public scannerForm(epsexecForm f)
        {
            InitializeComponent();
            mainForm = f;
        }
        bool canImport = false;
        private void ScanBtn_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            List<string> items = new List<string>(networkPCsBox.SelectedItems.Cast<string>().ToList());
            networkPCsBox.Items.Clear();
            foreach (string ip in items)
            {
                try
                {
                    tcpClient.Connect(ip, 445);
                    networkPCsBox.Items.Add($"{ip} Status: OPENED");
                    canImport = true;
                }
                catch (System.Net.Sockets.SocketException)
                {
                    if (!canImport)
                        canImport = false;
                    networkPCsBox.Items.Add($"{ip} Status: Closed");
                }
            }
            networkPCsBox.SelectionMode = SelectionMode.One;
            networkPCsBox.SelectedItem = null;
            if (canImport)
                importBtn.Enabled = true;
        }

        private void GetPCSBox_Click(object sender, EventArgs e)
        {
            networkPCsBox.SelectionMode = SelectionMode.MultiSimple;
            networkPCsBox.Items.Clear();
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c arp -a | findstr /v \"Internet Address\" | findstr /v \"Interface:\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            string lines = proc.StandardOutput.ReadToEnd();
            proc.Close();
            
            foreach (string line in lines.Split('\n'))
            {
                if (line.Length < 30)
                    continue;

                string newLine = line.Substring(2);
                networkPCsBox.Items.Add(newLine.Split(' ')[0]);
            }
            
            scanBtn.Enabled = true;
        }

        private void ImportBtn_Click(object sender, EventArgs e)
        {
            mainForm.ipBox.Text = ((string)networkPCsBox.SelectedItem).Substring(0,((string)networkPCsBox.SelectedItem).Length - 15);
            this.Close();
        }

        private void SelectAllBtn_Click(object sender, EventArgs e)
        {
            var temp = networkPCsBox.Items;
            if (networkPCsBox.SelectedIndices.Count == networkPCsBox.Items.Count)
            {
                networkPCsBox.SelectedIndices.Clear();
                return;
            }
            for (int i = 0; i < temp.Count; i++)
            {

                networkPCsBox.SelectedIndices.Add(i);
            }
        }
    }
}
