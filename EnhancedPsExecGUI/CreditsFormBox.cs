using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using EnhancedPsExecGUI;

namespace EnhancedPsExec
{
    public partial class CreditsFormBox : Form
    {
        epsexecForm mainForm;
        public CreditsFormBox(epsexecForm MainForm)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            mainForm = MainForm;
            
        }
        
        private void YtStart_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/c/hackinggaming");

        }

        private void GithubBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/orishamir");
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://pypi.org/project/Enhanced-PsExec/");
        }

        private void CreditsFormBox_Load(object sender, EventArgs e)
        {
            
            label1.Focus();
        }
    }
}
