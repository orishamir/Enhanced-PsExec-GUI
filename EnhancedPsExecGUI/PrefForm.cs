using EnhancedPsExecGUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnhancedPsExec
{
    public partial class PrefForm : Form
    {
        epsexecForm mainForm;
        string configPath;
        public PrefForm(epsexecForm main)
        {
            configPath = main.configPath;
            InitializeComponent();
            mainForm = main;

            IniParser.FileIniDataParser parser = new IniParser.FileIniDataParser();

            if (!File.Exists(configPath))
            {
                File.WriteAllLines(configPath, new string[]{
                    "; Settings",
                    "; Feel free to edit this",
                    "; Do not get rid of any setting.set it to true / false",
                    "",
                    "ignore_ip_warn                 = true",
                    "ignore_not_connected_warn      = false",
                    "always_on_top                  = false",
                    "hide_in_taskbar                = false",
                    "dont_use_paexec_on_remote_pc   = false"
                    });
            }

            IniParser.Model.IniData data = parser.ReadFile(configPath);

            ignoreEmptyIpBox.Checked = Boolean.Parse(data.GetKey("ignore_ip_warn"));
            ignoreNotConnectedBox.Checked = Boolean.Parse(data.GetKey("ignore_not_connected_warn"));
            this.alwaysOnTopBox.Checked = Boolean.Parse(data.GetKey("always_on_top"));
            hideInTaskbarBox.Checked = Boolean.Parse(data.GetKey("hide_in_taskbar"));
            noPaexecBox.Checked = Boolean.Parse(data.GetKey("dont_use_paexec_on_remote_pc"));

            if (mainForm.ignoreIpEmpty != ignoreEmptyIpBox.Checked)
                mainForm.ignoreIpEmpty = ignoreEmptyIpBox.Checked;

            if (mainForm.ignoreNotConnected != ignoreNotConnectedBox.Checked)
                mainForm.ignoreNotConnected = ignoreNotConnectedBox.Checked;

            if (mainForm.alwaysOnTopToolStripMenuItem.Checked != alwaysOnTopBox.Checked)
                mainForm.alwaysOnTopToolStripMenuItem.Checked = alwaysOnTopBox.Checked;

            if (mainForm.hideInTaskbarToolStripMenuItem.Checked != hideInTaskbarBox.Checked)
                mainForm.hideInTaskbarToolStripMenuItem.Checked = hideInTaskbarBox.Checked;

            if (mainForm.noPaExec != noPaexecBox.Checked)
                mainForm.noPaExec = noPaexecBox.Checked;
        }

        private void updateConfig(object sender, EventArgs e)
        {

            List<string> lines = new List<string>();
            lines.Add(";;;;; Settings");
            lines.Add(";   Feel free to edit this");
            lines.Add(";   Do not get rid of any setting. set it to true/false");
            lines.Add("");
            lines.Add("ignore_ip_warn                  = " + ignoreEmptyIpBox.Checked.ToString().ToLower());
            lines.Add("ignore_not_connected_warn       = " + ignoreNotConnectedBox.Checked.ToString().ToLower());
            lines.Add("always_on_top                   = " + alwaysOnTopBox.Checked.ToString().ToLower());
            lines.Add("hide_in_taskbar                 = " + hideInTaskbarBox.Checked.ToString().ToLower());
            lines.Add("dont_use_paexec_on_remote_pc    = " + noPaexecBox.Checked.ToString().ToLower());

            File.WriteAllLines(configPath, lines.ToArray());

            mainForm.ignoreIpEmpty = ignoreEmptyIpBox.Checked;
            mainForm.ignoreNotConnected = ignoreNotConnectedBox.Checked;

            mainForm.alwaysOnTopToolStripMenuItem.Checked = alwaysOnTopBox.Checked;
            mainForm.TopMost = alwaysOnTopBox.Checked;

            mainForm.hideInTaskbarToolStripMenuItem.Checked = hideInTaskbarBox.Checked;
            mainForm.ShowInTaskbar = !hideInTaskbarBox.Checked;

            mainForm.noPaExec = noPaexecBox.Checked;
            this.Focus();
        }
    }
}
