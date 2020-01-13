using System;
using System.Drawing;
using System.Windows.Forms;
using EnhancedPsExecGUI;
namespace EnhancedPsExec
{
    public partial class scriptingHowTo : Form
    {
        epsexecForm main;
        public scriptingHowTo(epsexecForm f)
        {
            main = f;
            InitializeComponent();
        }

        private void scriptingHowTo_FormClosing(object sender, FormClosingEventArgs e)
        {
            main.howto = null;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Focus();
        }

        private void scriptingHowTo_Load(object sender, EventArgs e)
        {
            //richTextBox1.ForeColor = Color.LightBlue;
            richTextBox1.AppendText("do");

            //richTextBox1.ForeColor = Color.White;
            richTextBox1.AppendText(" X ");

            //richTextBox1.ForeColor = Color.White;
            richTextBox1.AppendText(" times{ ");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("    func arg1, arg2\n");
            richTextBox1.AppendText("  click 750, 620\n");
            richTextBox1.AppendText("}");
        }

        private void scriptingHowTo_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("hello");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
