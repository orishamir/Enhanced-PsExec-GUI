using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using EnhancedPsExecGUI;
namespace EnhancedPsExec
{
    public partial class keyboard : Form
    {
        epsexecForm mainForm;
        string sendMe = "";
        bool ctrlClicked = false;
        bool altClicked = false;
        bool winClicked = false;
        bool shiftClicked = true;
        bool capsClicked = true;

        bool disableKeyboard = false;
        public keyboard(epsexecForm mf)
        {
            mainForm = mf;
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (disableKeyboard)
                return false;
            Console.WriteLine("key: " + (int)keyData);
            if ((int)keyData == 131089) {
                ctrlBtn_Click(new object(), new EventArgs());
                return false;
            }
            
            if (keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Down || keyData == Keys.Up)
            {
                Button b = new Button();
                if (keyData == Keys.Left)
                    b.Name = "leftBtn";

                else if (keyData == Keys.Right)
                    b.Name = "rightBtn";

                else if (keyData == Keys.Up)
                    b.Name = "upBtn";

                else if (keyData == Keys.Down)
                    b.Name = "downBtn";

                arrowsBtn_click(b, new EventArgs());
                return false;
            }
            if(keyData == Keys.LWin || keyData == Keys.RWin)
            {
                winKey_Click(new object(), new EventArgs());
                this.Focus();
                return false;
            }
            
            switch (keyData)
            {
                case Keys.Return:
                    this.enterBtn_Click(new object(), new EventArgs());
                    break;

                case Keys.Tab:
                    tabKey_Click(new object(), new EventArgs());
                    return false;
                    break;

                case Keys.Escape:
                    escBtn_Click(new object(), new EventArgs());
                    break;

                case Keys.Space:
                    return false;
                    break;
                
                case Keys.Alt:
                    altBtn_Click(new object(), new EventArgs());
                    
                    break;
                case Keys.Back:
                    try
                    {
                        sendMe = sendMe.TrimEnd();

                        char cc = sendMe[sendMe.Length - 1];
                        // cut down until a space is hit
                        while (cc != ' ' && cc != '+')
                        {
                            sendMe = sendMe.Substring(0, sendMe.Length - 1);
                            cc = sendMe[sendMe.Length - 1];
                        }
                        if (sendMe.EndsWith("+"))
                            sendMe = sendMe.Substring(0, sendMe.Length - 1);

                        sendBox.Text = sendMe;
                    }
                    catch (ArgumentOutOfRangeException) { }
                    catch (IndexOutOfRangeException) { }

                    return false;
                    break;
                
                

            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void keyboard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (disableKeyboard)
                return;
            if (e.KeyChar == ' ') {
                sendMe += " spc ";
                sendBox.Text = sendMe;
            }

            if ((e.KeyChar) == 91)
            {
                e.Handled = true;
            }
            if (e.KeyChar == 8 || e.KeyChar == 27) //|| e.KeyChar >= 112  && e.KeyChar <= 135)
                return;
            Button b = new Button();
            b.Text = e.KeyChar.ToString();
            letterBtn_clicked(b, e);
        }

        private void arrowsBtn_click(object sender, EventArgs e)
        {
            string name = ((Button)sender).Name;
            if (name == "upBtn")
                sendMe += " up ";
            
            else if (name == "downBtn")
                sendMe += " down ";
            
            else if (name == "leftBtn")
                sendMe += " left ";
            
            else if (name == "rightBtn")
                sendMe += " right ";

            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
        }

        private void keyboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (disableKeyboard)
                return;
            if (e.Shift)
            {
                shiftClicked = true;
                shiftBtns();
                return;
            }
            if (e.KeyValue == 20)
            {
                capsBtn_Click(sender, e);
                return;
            }
            // alt key
            if (e.KeyValue == 18)
            {
                altBtn_Click(sender, e);
                return;
            }

            // FXX keys, F1 is 112
            if (e.KeyValue <= 136 && e.KeyValue >= 112)
            {
                if (!ctrlClicked && !altClicked && !winClicked)
                    sendMe += $" F{e.KeyValue - 111} ";
                else
                    sendMe += $"F{e.KeyValue - 111}+";
                sendMe = sendMe.Replace("  ", " ");
                sendBox.Text = sendMe;
                return;
            }
        }
        private void fX_KeyDown(object sender, EventArgs e)
        {
           // Console.WriteLine((int)Keys.F1 + "-" + (int)Keys.F24);
            if (disableKeyboard)
                return;
            try
            {
                if (!ctrlClicked && !altClicked && !winClicked)
                    sendMe += $" {(((Button)sender).Text)} ";
                else
                    sendMe += $"{(((Button)sender).Text)}+";
                //sendMe = sendMe.Replace("  ", " ");
                sendBox.Text = sendMe;
            }
            catch (Exception){}
        }
        private void keyboard_KeyUp(object sender, KeyEventArgs e)
        {
            if (disableKeyboard)
                return;
            if (e.KeyValue == 16)
            {
                shiftClicked = false;
                unshiftBtns();
                return;
            }
        }

        private void unshiftBtns()
        {
            zeroBtn.Text = "0";
            oneBtn.Text = "1";
            twoBtn.Text = "2";
            threeBtn.Text = "3";
            fourBtn.Text = "4";
            fiveBtn.Text = "5";
            sixBtn.Text = "6";
            sevenBtn.Text = "7";
            eightBtn.Text = "8";
            nineBtn.Text = "9";
            // special
            idkBtn.Text = "`";
            minusBtn.Text = "-";
            equalBtn.Text = "=";
            colonBtn.Text = ";";
            psikBtn.Text = ",";
            dotBtn.Text = ".";
            slashBtn.Text = "/";
            // -------
            // do the abc
            aBtn.Text = aBtn.Text.ToLower();
            bBtn.Text = bBtn.Text.ToLower();
            cBtn.Text = cBtn.Text.ToLower();
            dBtn.Text = dBtn.Text.ToLower();
            eBtn.Text = eBtn.Text.ToLower();
            fBtn.Text = fBtn.Text.ToLower();
            gBtn.Text = gBtn.Text.ToLower();
            hBtn.Text = hBtn.Text.ToLower();
            iBtn.Text = iBtn.Text.ToLower();
            jBtn.Text = jBtn.Text.ToLower();
            kBtn.Text = kBtn.Text.ToLower();
            lBtn.Text = lBtn.Text.ToLower();
            mBtn.Text = mBtn.Text.ToLower();
            nBtn.Text = nBtn.Text.ToLower();
            oBtn.Text = oBtn.Text.ToLower();
            pBtn.Text = pBtn.Text.ToLower();
            qBtn.Text = qBtn.Text.ToLower();
            rBtn.Text = rBtn.Text.ToLower();
            sBtn.Text = sBtn.Text.ToLower();
            tBtn.Text = tBtn.Text.ToLower();
            uBtn.Text = uBtn.Text.ToLower();
            vBtn.Text = vBtn.Text.ToLower();
            wBtn.Text = wBtn.Text.ToLower();
            xBtn.Text = xBtn.Text.ToLower();
            yBtn.Text = yBtn.Text.ToLower();
            zBtn.Text = zBtn.Text.ToLower();

            lShiftBtn.BackColor = default(Color);
            rShiftBtn.BackColor = default;
        }
        private void shiftBtns()
        {
            // set 1234567890 keys to their normal things.
            zeroBtn.Text = ")";
            oneBtn.Text = "!";
            twoBtn.Text = "@";
            threeBtn.Text = "#";
            fourBtn.Text = "$";
            fiveBtn.Text = "%";
            sixBtn.Text = "^";
            sevenBtn.Text = "&&";
            eightBtn.Text = "*";
            nineBtn.Text = "(";
            // special
            idkBtn.Text = "~";
            minusBtn.Text = "_";
            equalBtn.Text = "+";
            colonBtn.Text = ":";
            psikBtn.Text = "<";
            dotBtn.Text = ">";
            slashBtn.Text = "?";
            //---
            // ABC
            aBtn.Text = aBtn.Text.ToUpper();
            bBtn.Text = bBtn.Text.ToUpper();
            cBtn.Text = cBtn.Text.ToUpper();
            dBtn.Text = dBtn.Text.ToUpper();
            eBtn.Text = eBtn.Text.ToUpper();
            fBtn.Text = fBtn.Text.ToUpper();
            gBtn.Text = gBtn.Text.ToUpper();
            hBtn.Text = hBtn.Text.ToUpper();
            iBtn.Text = iBtn.Text.ToUpper();
            jBtn.Text = jBtn.Text.ToUpper();
            kBtn.Text = kBtn.Text.ToUpper();
            lBtn.Text = lBtn.Text.ToUpper();
            mBtn.Text = mBtn.Text.ToUpper();
            nBtn.Text = nBtn.Text.ToUpper();
            oBtn.Text = oBtn.Text.ToUpper();
            pBtn.Text = pBtn.Text.ToUpper();
            qBtn.Text = qBtn.Text.ToUpper();
            rBtn.Text = rBtn.Text.ToUpper();
            sBtn.Text = sBtn.Text.ToUpper();
            tBtn.Text = tBtn.Text.ToUpper();
            uBtn.Text = uBtn.Text.ToUpper();
            vBtn.Text = vBtn.Text.ToUpper();
            wBtn.Text = wBtn.Text.ToUpper();
            xBtn.Text = xBtn.Text.ToUpper();
            yBtn.Text = yBtn.Text.ToUpper();
            zBtn.Text = zBtn.Text.ToUpper();
            //--
            lShiftBtn.BackColor = Color.LightBlue;
            rShiftBtn.BackColor = Color.LightBlue;
        }

        private void letterBtn_clicked(object sender, EventArgs e)
        {
            if(!ctrlClicked && !altClicked && !winClicked)
                sendMe += $" {(((Button)sender).Text)[0]} ";
            else
                sendMe += $"{(((Button)sender).Text)[0]}+";
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
            focusThis.Focus();
        }

        private void spaceBtn_Click(object sender, EventArgs e)
        {
            sendMe += " spc ";
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
            focusThis.Focus();
        }

        private void liveBox_CheckedChanged(object sender, EventArgs e)
        {
            sendBtn.Visible = !liveBox.Checked;
            focusThis.Focus();
        }

        private void ctrlBtn_Click(object sender, EventArgs e)
        {
            ctrlClicked = !ctrlClicked;
            if (ctrlClicked)
                sendMe += " ctrl+";
            else
                sendMe = sendMe.Substring(0, sendMe.Length-1);
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
            
            focusThis.Focus();            
        }

        private void shiftKey_Click(object sender, EventArgs e)
        {
            if (shiftClicked)
                shiftBtns();
            else
                unshiftBtns();
            focusThis.Focus();
            shiftClicked = !shiftClicked;
        }

        private void capsBtn_Click(object sender, EventArgs e)
        {
            if (capsClicked)
            {
                capsBtn.BackColor = Color.LightBlue;
                //---
                // ABC
                aBtn.Text = aBtn.Text.ToUpper();
                bBtn.Text = bBtn.Text.ToUpper();
                cBtn.Text = cBtn.Text.ToUpper();
                dBtn.Text = dBtn.Text.ToUpper();
                eBtn.Text = eBtn.Text.ToUpper();
                fBtn.Text = fBtn.Text.ToUpper();
                gBtn.Text = gBtn.Text.ToUpper();
                hBtn.Text = hBtn.Text.ToUpper();
                iBtn.Text = iBtn.Text.ToUpper();
                jBtn.Text = jBtn.Text.ToUpper();
                kBtn.Text = kBtn.Text.ToUpper();
                lBtn.Text = lBtn.Text.ToUpper();
                mBtn.Text = mBtn.Text.ToUpper();
                nBtn.Text = nBtn.Text.ToUpper();
                oBtn.Text = oBtn.Text.ToUpper();
                pBtn.Text = pBtn.Text.ToUpper();
                qBtn.Text = qBtn.Text.ToUpper();
                rBtn.Text = rBtn.Text.ToUpper();
                sBtn.Text = sBtn.Text.ToUpper();
                tBtn.Text = tBtn.Text.ToUpper();
                uBtn.Text = uBtn.Text.ToUpper();
                vBtn.Text = vBtn.Text.ToUpper();
                wBtn.Text = wBtn.Text.ToUpper();
                xBtn.Text = xBtn.Text.ToUpper();
                yBtn.Text = yBtn.Text.ToUpper();
                zBtn.Text = zBtn.Text.ToUpper();
            }
            else
            {
                capsBtn.BackColor = default(Color);
                //abc
                aBtn.Text = aBtn.Text.ToLower();
                bBtn.Text = bBtn.Text.ToLower();
                cBtn.Text = cBtn.Text.ToLower();
                dBtn.Text = dBtn.Text.ToLower();
                eBtn.Text = eBtn.Text.ToLower();
                fBtn.Text = fBtn.Text.ToLower();
                gBtn.Text = gBtn.Text.ToLower();
                hBtn.Text = hBtn.Text.ToLower();
                iBtn.Text = iBtn.Text.ToLower();
                jBtn.Text = jBtn.Text.ToLower();
                kBtn.Text = kBtn.Text.ToLower();
                lBtn.Text = lBtn.Text.ToLower();
                mBtn.Text = mBtn.Text.ToLower();
                nBtn.Text = nBtn.Text.ToLower();
                oBtn.Text = oBtn.Text.ToLower();
                pBtn.Text = pBtn.Text.ToLower();
                qBtn.Text = qBtn.Text.ToLower();
                rBtn.Text = rBtn.Text.ToLower();
                sBtn.Text = sBtn.Text.ToLower();
                tBtn.Text = tBtn.Text.ToLower();
                uBtn.Text = uBtn.Text.ToLower();
                vBtn.Text = vBtn.Text.ToLower();
                wBtn.Text = wBtn.Text.ToLower();
                xBtn.Text = xBtn.Text.ToLower();
                yBtn.Text = yBtn.Text.ToLower();
                zBtn.Text = zBtn.Text.ToLower();
            }
            capsClicked = !capsClicked;
            focusThis.Focus();
        }

        private void enterBtn_Click(object sender, EventArgs e)
        {
            sendMe += " enter ";
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
            focusThis.Focus();
        }

        private void altBtn_Click(object sender, EventArgs e)
        {
            altClicked = !altClicked;
            if (altClicked)
                sendMe += " alt+";
            else
                sendMe = sendMe.Substring(0, sendMe.Length - 1);
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;

            focusThis.Focus();
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            string sendMeFR = sendMe.Replace("  ", " ")
                .Replace(" = ", " 0xBB ")
                .Replace(" + ", " shift+0xBB ")

                .Replace(" ` ", " 0xC0 ")
                .Replace(" ~ ", " shift+0xC0 ")

                .Replace("!", " shift+1 ")
                .Replace("@", " shift+2 ")
                .Replace("#", " shift+3 ")
                .Replace("$", " shift+4 ")
                .Replace("%", " shift+5 ")
                .Replace("^", " shift+6 ")
                .Replace("&", " shift+7 ")
                .Replace("*", " shift+8 ")
                .Replace("(", " shift+9 ")
                .Replace(")", " shift+0 ")
                
                .Replace(" - ", " 0xBD ")
                .Replace(" _ ", " shift+0xBD ")
                
                .Replace(" ; ", " 0xBA ")
                .Replace(" : ", " shift+0xBA ")

                .Replace(" / ", " 0xBF ")
                .Replace(" ? ", " shift+0xBF ")

                .Replace(" \\ ", " 0xE2 ")
                .Replace(" | ", " shift+0xE2 ")

                .Replace(" , ", " 0xBC ")
                .Replace(" < ", " shift+0xBC ")

                .Replace(" . ", " 0xBE ")
                .Replace(" > ", " shift+0xBE ")
               
                // abc
                .Replace(" A ", " shift+a ")
                .Replace(" B ", " shift+b ")
                .Replace(" C ", " shift+c ")
                .Replace(" D ", " shift+d ")
                .Replace(" E ", " shift+e ")
                .Replace(" F ", " shift+f ")
                .Replace(" G ", " shift+g ")
                .Replace(" H ", " shift+h ")
                .Replace(" I ", " shift+i ")
                .Replace(" J ", " shift+j ")
                .Replace(" K ", " shift+k ")
                .Replace(" L ", " shift+l ")
                .Replace(" M ", " shift+m ")
                .Replace(" N ", " shift+n ")
                .Replace(" O ", " shift+o ")
                .Replace(" P ", " shift+p ")
                .Replace(" Q ", " shift+q ")
                .Replace(" R ", " shift+r ")
                .Replace(" S ", " shift+s ")
                .Replace(" T ", " shift+t ")
                .Replace(" U ", " shift+u ")
                .Replace(" V ", " shift+v ")
                .Replace(" W ", " shift+w ")
                .Replace(" X ", " shift+x ")
                .Replace(" Y ", " shift+y ")
                .Replace(" Z ", " shift+z ")
                ;
            Console.WriteLine("sendme: " + sendMe);
            Console.WriteLine("sendmeFR: " + sendMeFR);
            
            focusThis.Focus();
            Thread.Sleep(5000);
            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{mainForm.ipBox.Text} -u {mainForm.usrBox.Text} -p {mainForm.passwordBox.Text} -s -i -accepteula nircmd.exe sendkeypress {sendMeFR}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc2.Start();
            proc2.Close();
        }

        private void winKey_Click(object sender, EventArgs e)
        {
            /* sendMe += " lwin ";
             sendMe = sendMe.Replace("  ", " ");
             sendBox.Text = sendMe;
             focusThis.Focus();*/
            // win key: 91
            winClicked = !winClicked;
            if (winClicked)
                sendMe += " win+";
            else
                sendMe = sendMe.Substring(0, sendMe.Length - 1);
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;

            focusThis.Focus();
        }

        private void tabKey_Click(object sender, EventArgs e)
        {
            sendMe += " tab ";
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
            focusThis.Focus();
        }
        private void sendBox_TextChanged(object sender, EventArgs e)
        {
            focusThis.Focus();
        }

        private void equalBtn_Click(object sender, EventArgs e)
        {
            sendMe += $" {equalBtn.Text} ";
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
            focusThis.Focus();
        }

        private void minusBtn_Click(object sender, EventArgs e)
        {
            sendMe += $" {minusBtn.Text} ";
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
            focusThis.Focus();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            sendMe = "";
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
            focusThis.Focus();
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

        private void disableKeyboardInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableKeyboard = disableKeyboardInputToolStripMenuItem.Checked;
        }

        private void sendBox_MouseHover(object sender, EventArgs e)
        {
            this.focusThis.Focus();
        }

        private void sendBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Console.WriteLine("yea");
                Clipboard.SetText(sendBox.Text);
                MessageBox.Show("Copied To Clipboard");

            }
            this.focusThis.Focus();
        }

        private void keyboard_Click(object sender, EventArgs e)
        {
            focusThis.Focus();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            Button b = new Button();
            b.Name = ((ComboBox)sender).Name;
            b.Text = ((ComboBox)sender).Text;

            fX_KeyDown(b, e);
        }

        private void escBtn_Click(object sender, EventArgs e)
        {
            sendMe += " esc ";
            sendMe = sendMe.Replace("  ", " ");
            sendBox.Text = sendMe;
            focusThis.Focus();
        }

        private void backspaceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                sendMe = sendMe.TrimEnd();
                
                char cc = sendMe[sendMe.Length - 1];
                // cut down until a space is hit
                while (cc != ' ' && cc != '+')
                {
                    sendMe = sendMe.Substring(0, sendMe.Length - 1);
                    cc = sendMe[sendMe.Length - 1];
                }
                if (sendMe.EndsWith("+"))
                    sendMe = sendMe.Substring(0, sendMe.Length - 1);

                sendBox.Text = sendMe;
            }
            catch (ArgumentOutOfRangeException) { }
            catch (IndexOutOfRangeException) { }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("blululu: " + e.KeyChar);
        }
    }
}
