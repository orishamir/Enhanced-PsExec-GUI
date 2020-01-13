using System;
using System.Windows.Forms;
using System.Diagnostics;
using EnhancedPsExecGUI;

namespace EnhancedPsExec
{
    public partial class CreditsForm : Form
    {
        epsexecForm mainForm;
        public CreditsForm(epsexecForm MainForm)
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
            Process.Start("https://pypi.org/project/Enhanced-PsExec/");
        }

        private void CreditsFormBox_Load(object sender, EventArgs e)
        {

            focusThis.Focus();
        }

        private void focusIt(object sender, KeyEventArgs e)
        {
            focusThis.Focus();
        }

        private void focusIt(object sender, KeyPressEventArgs e)
        {
            focusThis.Focus();
        }

        private void focusIt(object sender, MouseEventArgs e)
        {
            focusThis.Focus();
        }

        private void focusIt(object sender, EventArgs e)
        {
            focusThis.Focus();
        }
    }
}
