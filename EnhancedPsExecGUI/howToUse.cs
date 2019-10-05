using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnhancedPsExec
{
    public partial class howToUse : Form
    {
        public howToUse()
        {
            InitializeComponent();
        }

        private void HowToUse_Load(object sender, EventArgs e)
        {
            
            adminEnableBox.Text = "net user /add usernameToHack passToBeUsed\r\nnet localgroup administrators usernameToHack /add";
            
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.microsoft.com/en-us/sysinternals/downloads/psexec");
        }

        private void LogoHowToUseLbl_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/orishamir/");
        }

        private void nircmdLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nirsoft.net/utils/nircmd.html");
        }
    }
}
