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
using Microsoft.VisualBasic;
using EnhancedPsExec;
using System.IO;
using System.Security;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.Speech.Recognition;

namespace EnhancedPsExecGUI
{

    public partial class epsexecForm : Form
    {

        public epsexecForm()
        {
            InitializeComponent();
            //Create right click menu..
            ContextMenuStrip s = new ContextMenuStrip();

            // add one right click menu item named as hello           
            ToolStripMenuItem load = new ToolStripMenuItem();
            ToolStripMenuItem save = new ToolStripMenuItem();
            alwaysOnTop  = new ToolStripMenuItem();
            load.Text = "Load";
            load.Image = this.loadSetting.Image;

            save.Text = "Save";
            save.Image = this.saveToolStripMenuItem.Image;

            alwaysOnTop.Text = "Always On Top";
            alwaysOnTop.Image = this.alwaysOnTopToolStripMenuItem.Image;
            // add the clickevent of hello item
            load.Click += (o, e) => loadToolStripMenuItem.PerformClick();
            save.Click += (o, e) => saveToolStripMenuItem.PerformClick();
            alwaysOnTop.Click += (o, e) =>
            {
                this.TopMost = !this.TopMost;
                alwaysOnTop.Checked = this.TopMost;
                alwaysOnTopToolStripMenuItem.Checked = this.TopMost;
            };
            // add the item in right click menu
            s.Items.Add(load);
            s.Items.Add(save);
            s.Items.Add(alwaysOnTop);
            // attach the right click menu with form
            this.ContextMenuStrip = s;
        }
        ToolStripMenuItem alwaysOnTop;
        string IP = "";
        string username = "";
        string password = "";
        string fileName = "c21f969b5f03d33d43e04f8f136e7682";
        bool fileJustGotEdited = false;
        SpeechRecognitionEngine recog = new SpeechRecognitionEngine();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(epsexecForm));
        private void Form1_Load(object sender, EventArgs e)
        {
            recog.LoadGrammarAsync(new DictationGrammar());
            recog.SetInputToDefaultAudioDevice();
            recog.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recog_SpeechRecognized);

            void loadFile(string filePath)
            {

                string[] lines = File.ReadAllLines(filePath);
                bool run = false;
                foreach (string line in lines)
                {
                    if (line.StartsWith("ip="))
                        ipBox.Text = line.Substring(line.IndexOf("ip=") + 3);

                    if (line.StartsWith("username="))
                        usrBox.Text = line.Substring(line.IndexOf("username=") + 9);

                    if (line.StartsWith("password="))
                        passwordBox.Text = line.Substring(line.IndexOf("password=") + 9);

                    if (line.StartsWith("url="))
                        urlBox.Text = line.Substring(line.IndexOf("url=") + 4);

                    if (line.StartsWith("tabs="))
                    {
                        ushort tbs = ushort.Parse(line.Substring(line.IndexOf("tabs=") + 5));
                        tabsBox.Value = tbs < 1 || tbs > ushort.MaxValue ? 1 : tbs;
                    }

                    if (line.StartsWith("delaybefore="))
                        delayBeforeBox.Value = ushort.Parse(line.Substring(line.IndexOf("delaybefore=") + 12));

                    if (line.StartsWith("delaybetween="))
                        delayBetweenBox.Value = ushort.Parse(line.Substring(line.IndexOf("delaybetween=") + 13));

                    // after checking case-sensitive parts, we can get the party started.
                    string lineL = line.ToLower();

                    if (lineL == "invisible" || lineL == "invisible=true")
                        invisibleBox.Checked = true;

                    if (lineL == "incognito" || lineL == "incognito=true")
                        incognitoBox.Checked = true;

                    if (lineL == "openurl" || lineL == "openurl=yes")
                        run = true;

                    if (lineL == "newwindow" || lineL == "newwindow=true" || lineL == "nw=true")
                        newWindowBox.Checked = true;

                    // Close Process
                    if (line.StartsWith("processes=("))
                    {
                        killBtn.Enabled = true;
                        string processes = line.Substring(11, line.Substring(11).Length - 2);

                        foreach (string prcs in processes.Split(','))
                        {
                            processesBox.Items.Add(prcs);
                        }
                    }

                    if (lineL == "caseinsensitive=true")
                        caseInsensitiveBox.Checked = true;

                    if (lineL == "autoclear=true")
                        autoClearBox.Checked = true;

                    if (lineL == "everythingexcept=true")
                        excludeBox.Checked = true;

                    // SOUND
                    if (line.StartsWith("volume="))
                    {
                        volumeBox.Value = ushort.Parse(line.Substring(line.IndexOf("volume=") + 7));
                        percentLabel.Text = $"{volumeBox.Value}%";
                    }

                    //Mouse&Keyboard
                    if (line.StartsWith("mouseX="))
                        moveMouseXBox.Text = line.Substring(line.IndexOf("mouseX=") + 7);

                    if (line.StartsWith("mouseY="))
                        moveMouseYBox.Text = line.Substring(line.IndexOf("mouseY=") + 7);

                    // misc2
                    if (line.StartsWith("copyLocation="))
                        getFileLocationBox.Text = line.Substring(line.IndexOf("copyLocation=") + 13);

                    if (line.StartsWith("copyToLocation="))
                        saveToBox.Text = line.Substring(line.IndexOf("copyToLocation=") + 15);
                }
                if (run)
                    RunBtn_Click(sender, e);
            }
            string[] passedInArgs = Environment.GetCommandLineArgs();

            if (passedInArgs.Length > 1)
            {
                fileName = passedInArgs[1];
                this.Text = $"{fileName.Split('\\').Last()} - Control Panel";
                this.saveToolStripMenuItem.Text = $"Save {fileName.Split('\\').Last()}";
                loadFile(fileName);
            }
            fileJustGotEdited = false;
        }

        private void RunBtn_Click(object sender, EventArgs e)
        {

            IP = ipBox.Text;
            username = usrBox.Text;
            password = passwordBox.Text;

            string url = urlBox.Text;
            ushort tabs = (ushort)tabsBox.Value;

            ushort delayBefore = (ushort)delayBeforeBox.Value;
            ushort delayBetween = (ushort)delayBetweenBox.Value;

            string incognito = incognitoBox.Checked ? "-i" : "";
            string newWindow = newWindowBox.Checked ? "-nw" : "";
            string invisible = invisibleBox.Checked ? "-invis" : "";
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "openurl.exe",
                    Arguments = $"-ip {IP} -u {username} -p {password} -url {url} -n {tabs} -dbt {delayBetween} -dbf {delayBefore} {incognito} {invisible} {newWindow}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            proc.Close();
        }


        private void TerminateChromeBtn_Click(object sender, EventArgs e)
        {
            IP = ipBox.Text;
            username = usrBox.Text;
            password = passwordBox.Text;

            ushort delay = (ushort)delayTerminate.Value;
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{IP} -u {username} -p {password} cmd.exe /c taskkill /F /IM chrome.exe /T",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            Thread.Sleep((ushort)delayTerminate.Value);
            proc.Start();
            proc.Close();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void CreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreditsFormBox creditsFormBox = new CreditsFormBox(this);
            creditsFormBox.Show();

        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();
            // Home tab
            if (ipBox.Text != "")
                lines.Add($"ip={ipBox.Text}");
            if (usrBox.Text != "")
                lines.Add($"username={usrBox.Text}");
            if (passwordBox.Text != "")
                lines.Add($"password={passwordBox.Text}");
            lines.Add("\n\n");

            // URL tab
            if (urlBox.Text != "")
                lines.Add($"url={urlBox.Text}");
            if (tabsBox.Value > 1)
                lines.Add($"tabs={tabsBox.Value}");

            if (invisibleBox.Checked)
                lines.Add("invisible=true");
            if (incognitoBox.Checked)
                lines.Add("incognito=true");
            if (newWindowBox.Checked)
                lines.Add("newwindow=true");

            if (delayBeforeBox.Value != 100 && delayBeforeBox.Value >= 0)
                lines.Add($"delaybefore={delayBeforeBox.Value}");

            if (delayBetweenBox.Value != 100 && delayBetweenBox.Value >= 0)
                lines.Add($"delaybetween={delayBetweenBox.Value}");
            lines.Add("\n\n");
            // Process Control
            if (processesBox.Visible)
            {
                lines.Add($"processes=(");
                int i = 0;
                foreach (string lineTaskToKill in processesBox.Items)
                {
                    if (i != 0 && i != 1)
                    {
                        string taskToKill = lineTaskToKill.Split(' ')[0];
                        if (i == processesBox.Items.Count - 1)
                            lines[lines.IndexOf(lines.Last())] += $"{taskToKill}) ";
                        else
                            lines[lines.IndexOf(lines.Last())] += $"{taskToKill},";
                    }
                    i++;
                }

                if (autoClearBox.Checked)
                    lines.Add("autoclear=true");

                if (caseInsensitiveBox.Checked)
                    lines.Add("caseinsensitive=true");

                if (excludeBox.Checked)
                    lines.Add("everythingexcept=true");
            }
            lines.Add("\n\n");
            // SOUND
            if (volumeBox.Value != 70)
                lines.Add($"volume={volumeBox.Value}");

            lines.Add("\n\n");
            // Mouse&Keyboard
            if (moveMouseXBox.Text != "")
                lines.Add($"mouseX={moveMouseXBox.Text}");

            if (moveMouseYBox.Text != "")
                lines.Add($"mouseY={moveMouseYBox.Text}");
            lines.Add("\n\n");


            //remote copy
            if (saveToBox.Text.Length > 1)
                lines.Add("copyToLocation=" + saveToBox.Text);

            if (getFileLocationBox.Text.Length > 1)
                lines.Add("copyLocation=" + getFileLocationBox.Text);
            lines.Add("\n\n");

            // KEEP THIS AT END
            if (fileName != "c21f969b5f03d33d43e04f8f136e7682")
            {
                fileJustGotEdited = false;
                File.WriteAllLines(fileName, lines);
                return;
            }
            using (SaveFileDialog fbd = new SaveFileDialog())
            {
                fbd.Filter = "Enhanced Psexec files | *.epsx";
                fbd.FileName = "config.epsx";
                fbd.Title = "Save A epsx file";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string filepath = fbd.FileName;
                    fileName = filepath;
                    File.WriteAllLines(filepath, lines);
                }

                this.Text = (fileName == "c21f969b5f03d33d43e04f8f136e7682") ? "Enhanced-PsExec Control Panel" : $"{fileName.Split('\\').Last()} - Control Panel";
                this.saveToolStripMenuItem.Text = (fileName == "c21f969b5f03d33d43e04f8f136e7682") ? "Save as" : $"Save {fileName.Split('\\').Last()}";

            }
            fileJustGotEdited = false;
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string fileName = Interaction.InputBox("Enter the file name:", "Load Config", "config.txt");
            string filePath = "";


            using (OpenFileDialog fbd = new OpenFileDialog())
            {
                fbd.Filter = "Enhanced Psexec File | *.epsx";
                fbd.Title = "Load A epsx File type";

                if (fbd.ShowDialog() == DialogResult.OK)
                    filePath = fbd.FileName.ToString();
                else
                    return;
            }

            string[] lines = File.ReadAllLines(filePath);
            bool run = false;
            foreach (string line in lines)
            {

                if (line.StartsWith("ip="))
                    ipBox.Text = line.Substring(line.IndexOf("ip=") + 3);

                if (line.StartsWith("username="))
                    usrBox.Text = line.Substring(line.IndexOf("username=") + 9);

                if (line.StartsWith("password="))
                    passwordBox.Text = line.Substring(line.IndexOf("password=") + 9);

                if (line.StartsWith("url="))
                    urlBox.Text = line.Substring(line.IndexOf("url=") + 4);

                if (line.StartsWith("tabs="))
                {
                    ushort tbs = ushort.Parse(line.Substring(line.IndexOf("tabs=") + 5));
                    tabsBox.Value = tbs < 1 || tbs > ushort.MaxValue ? 1 : tbs;
                }

                if (line.StartsWith("delaybefore="))
                    delayBeforeBox.Value = ushort.Parse(line.Substring(line.IndexOf("delaybefore=") + 12));

                if (line.StartsWith("delaybetween="))
                    delayBetweenBox.Value = ushort.Parse(line.Substring(line.IndexOf("delaybetween=") + 13));

                // after checking case-sensitive parts, we can get the party started.
                string lineL = line.ToLower();

                if (lineL == "invisible" || lineL == "invisible=true")
                    invisibleBox.Checked = true;

                if (lineL == "incognito" || lineL == "incognito=true")
                    incognitoBox.Checked = true;

                if (lineL == "openurl" || lineL == "openurl=yes")
                    run = true;

                if (lineL == "newwindow" || lineL == "newwindow=true" || lineL == "nw=true")
                    newWindowBox.Checked = true;

                // Close Process
                if (line.StartsWith("processes=("))
                {
                    killBtn.Visible = true;
                    string processes = line.Substring(11, line.Substring(11).Length - 2);

                    foreach (string prcs in processes.Split(','))
                    {
                        processesBox.Items.Add(prcs);
                    }
                }

                if (lineL == "caseinsensitive=true")
                    caseInsensitiveBox.Checked = true;

                if (lineL == "autoclear=true")
                    autoClearBox.Checked = true;

                if (lineL == "everythingexcept=true")
                    excludeBox.Checked = true;

                // SOUND
                if (line.StartsWith("volume="))
                {
                    volumeBox.Value = ushort.Parse(line.Substring(line.IndexOf("volume=") + 7));
                    percentLabel.Text = $"{volumeBox.Value}%";
                }

                //Mouse&Keyboard
                if (line.StartsWith("mouseX="))
                    moveMouseXBox.Text = line.Substring(line.IndexOf("mouseX=") + 7);

                if (line.StartsWith("mouseY="))
                    moveMouseYBox.Text = line.Substring(line.IndexOf("mouseY=") + 7);

                // misc2
                if (line.StartsWith("copyToLocation="))
                    saveToBox.Text = line.Substring(line.IndexOf("copyToLocation=") + 15);

                if (line.StartsWith("copyLocation="))
                    getFileLocationBox.Text = line.Substring(line.IndexOf("copyLocation=") + 13);

            }
            fileJustGotEdited = false;
            fileName = filePath;
            if (run)
                RunBtn_Click(sender, e);
        }

        private void any_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void any_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            MessageBox.Show("This feature is not and will not be developed\nsince you need to go through hell to drag and drop with administrative privileges.\nVisit: https://stackoverflow.com/a/50665247/9100289 ");
        }

        private void SaveIcon_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem.PerformClick();
        }

        private void LoadSetting_Click(object sender, EventArgs e)
        {
            loadToolStripMenuItem.PerformClick();
        }

        private void VolumeBox_Scroll(object sender, EventArgs e)
        {
            percentLabel.Text = $"{volumeBox.Value}%";
        }

        private void MuteLabel_Click(object sender, EventArgs e)
        {

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -d -s -accepteula nircmd.exe cmdwait {soundDelayBox.Value} nircmd mutesysvolume 1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            proc.Close();

        }

        private void UnmuteLabel_Click(object sender, EventArgs e)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -d -s -accepteula nircmd.exe cmdwait {soundDelayBox.Value} nircmd mutesysvolume 0",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            proc.Close();
        }

        private void SoundRunBtn_Click(object sender, EventArgs e)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -d -s -accepteula nircmd.exe cmdwait {soundDelayBox.Value} nircmd setsysvolume {655 * volumeBox.Value}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            proc.Close();
        }

        private void GetReloadBtn_Click(object sender, EventArgs e)
        {
            fileJustGotEdited = true;
            // get processes
            if (autoClearBox.Checked)
                processesBox.Items.Clear();

            string filterTextRule = "";
            if (filterTextBox.Text != "")
            {
                filterTextRule += "| findstr";
                if (caseInsensitiveBox.Checked)
                    filterTextRule += " /I ";
                if (excludeBox.Checked)
                    filterTextRule += " /V ";
                else
                {
                    processesBox.Items.Add("\n\nProcess Name                  PID");
                    processesBox.Items.Add("=====================================================================");
                }
                filterTextRule += $" \"{filterTextBox.Text}\" ";

            }
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    //Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -i cmd.exe /c tasklist {filterConditions} & echo. & echo Copy your process(es) & pause >nul",
                    //Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s tasklist {filterTextRule}",
                    Arguments = $"/c psexec.exe \\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s tasklist {filterTextRule}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if (line != "\n" && line != "PsExec v2.2 - Execute processes remotely" && line != "Copyright (C) 2001-2016 Mark Russinovich" && line != "Sysinternals - www.sysinternals.com")
                    processesBox.Items.Add(line);
            }
            proc.Close();
            processesBox.Visible = true;
            processesLabel.Visible = true;
            killBtn.Enabled = true;

        }

        private void KillBtn_Click(object sender, EventArgs e)
        {

            foreach (string lineTaskToKill in processesBox.SelectedItems)
            {
                string taskToKill = lineTaskToKill.Split(' ')[0];

                var proc2 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "psexec.exe",
                        Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s cmd.exe /c taskkill /F /IM {taskToKill}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,

                        CreateNoWindow = true
                    }
                };
                Thread.Sleep((ushort)processDelay.Value);
                proc2.Start();
                proc2.Close();

            }

        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            processesBox.Items.Clear();
        }

        private void HowToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            howToUse howToUseForm = new howToUse();
            howToUseForm.Show();
        }


        private void TtsRunBtn_Click(object sender, EventArgs e)
        {
            string maleVoice = maleVoiceBox.Checked ? "-s" : "";
            string toSay = ttsBox.Text;
            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -d -i {maleVoice} -accepteula nircmd.exe cmdwait {ttsDelayBox.Value} speak text \"{ttsBox.Text}\" {ttsSpeedBox.Value} {ttsVolumeBox.Value}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc2.Start();
            proc2.Close();
        }

        private void runMouseClick(object sender, EventArgs e)
        {
            var name = (Button)sender;
            string buttonType = name.Name;
            string leftRightMiddle = "";

            if (buttonType == "mouseFullClickBtn")
                buttonType = "click";
            else if (buttonType == "doubleClickBtn")
                buttonType = "dblclick";
            else if (buttonType == "downClickBtn")
                buttonType = "down";
            else if (buttonType == "upClickBtn")
                buttonType = "up";

            if (leftMouseBtn.Checked)
                leftRightMiddle = "left";
            else if (rightMouseBtn.Checked)
                leftRightMiddle = "right";
            else if (middleMouseBtn.Checked)
                leftRightMiddle = "middle";

            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -i -accepteula nircmd.exe cmdwait {clickDelayBox.Value} sendmouse {leftRightMiddle} {buttonType}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc2.Start();
            proc2.Close();
        }

        private void MoveBtn_Click(object sender, EventArgs e)
        {
            string moveX = moveMouseXBox.Text;
            string moveY = moveMouseYBox.Text;

            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -i -accepteula nircmd.exe cmdwait {moveMouseDelayBox.Value} setcursor {moveX} {moveY}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };

            proc2.Start();
            proc2.Close();
        }

        private void SendKeyboardTextBtn_Click(object sender, EventArgs e)
        {
            string sendMe = "";

            int i = 0;
            foreach (char c in sendKeyboardBox.Text)
            {
                if (i % 15 == 0)
                    sendMe += " ";
                switch (c)
                {
                    case '\r':
                        sendMe += "+enter+";
                        break;
                    case ' ':
                        sendMe += "spc+";
                        break;


                    case '!':
                        sendMe += " shift+1 ";
                        break;
                    case '@':
                        sendMe += " shift+2 ";
                        break;
                    case '#':
                        sendMe += " shift+3 ";
                        break;
                    case '%':
                        sendMe += " shift+5 ";
                        break;
                    case '^':
                        sendMe += " shift+6 ";
                        break;
                    case '&':
                        sendMe += " shift+7 ";
                        break;
                    case '*':
                        sendMe += " shift+8 ";
                        break;
                    case '(':
                        sendMe += " shift+9 ";
                        break;
                    case ')':
                        sendMe += " shift+0 ";
                        break;


                    default:
                        if (c != '\n')
                            if (Encoding.ASCII.GetBytes(c.ToString())[0] >= 65 && Encoding.ASCII.GetBytes(c.ToString())[0] <= 90)
                                sendMe += $" shift+{c} ";
                            else
                                sendMe += $"{c}+";
                        else
                            sendMe += "";
                        break;
                }
                i++;
            }
            // 15
            sendMe = sendMe.Replace("\n", "");
            sendMe = sendMe.Replace("\r", "");
            //Console.WriteLine(sendMe);

            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -i -accepteula nircmd.exe cmdwait {sendKeyboardDelayBox.Value} sendkeypress {sendMe}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc2.Start();
            proc2.Close();
        }

        private void DownloadNirBtn_Click(object sender, EventArgs e)
        {
            string powershellCommand = "wget \"\"\"http://www.nirsoft.net/utils/nircmd-x64.zip\"\"\" -OutFile C:\\windows\\system32\\nircmd.zip; ";
            powershellCommand += "Expand-Archive -Force C:\\windows\\system32\\nircmd.zip -DestinationPath C:\\windows\\system32; ";
            powershellCommand += "del C:\\windows\\system32\\nircmd.zip";

            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -d -accepteula powershell.exe /c {powershellCommand}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc2.Start();
            proc2.Close();
        }

        private void RunBeepSoundBtn_Click(object sender, EventArgs e)
        {
            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -d -i -accepteula nircmd.exe beep {frequencySoundBox.Value} {durationSoundBox.Value}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc2.Start();
            proc2.Close();
        }

        private void OpenScannerBtn_Click(object sender, EventArgs e)
        {
            scannerForm scForm = new scannerForm(this);
            scForm.Show();
        }

        private void AlwaysOnTopToolStripMenuItem_change(object sender, EventArgs e)
        {
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
            alwaysOnTop.Checked = this.TopMost;
            
        }

        private void NircmdAboutLabel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("NirCmd is a command-line utility that allows you to do some useful tasks\nWithout displaying any user interface And it is required for all of the features in the Misc tab.", "NirCmd?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FirewallRun_Click(object sender, EventArgs e)
        {
            string network_command = "";
            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -d -i -accepteula {network_command}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };

            if (networkOffBox.Checked)
            {
                network_command = "netsh advfirewall set allprofiles state off";
                proc2.Start();
                proc2.Close();
            }
            else if (networkOnBox.Checked)
            {
                network_command = "netsh advfirewall set allprofiles state on";
                proc2.Start();
                proc2.Close();
            }
            else if (networkSmbBox.Checked)
            {
                network_command = "netsh advfirewall firewall set rule group=\"File and Printer Sharing(SMB-In)\" new profile=private & netsh advfirewall firewall set rule name=\"File and Printer Sharing(SMB-In)\" dir=in new enable=Yes";
                proc2.Start();
                proc2.Close();
            }
        }

        private void EpsexecForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileName == "c21f969b5f03d33d43e04f8f136e7682" || fileJustGotEdited)
            {
                var result = MessageBox.Show("Would you like to save your file?", "You have unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    saveToolStripMenuItem.PerformClick();
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;

            }
        }
        private void on_Edit(object sender, EventArgs e)
        {
            fileJustGotEdited = true;
        }

        private void SpeakNowBtn_Click(object sender, EventArgs e)
        {
            if (speakNowBtn.Text == "Stop")
            {
                recog.RecognizeAsyncStop();
                speakNowBtn.Text = "Enter By saying";
            }
            else
            {
                speakNowBtn.Text = "Stop";
                recog.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void recog_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            sendKeyboardBox.Text = e.Result.Text;
        }

        private void getFile_Click(object sender, EventArgs e)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    //Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -i cmd.exe /c tasklist {filterConditions} & echo. & echo Copy your process(es) & pause >nul",
                    //Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s tasklist {filterTextRule}",
                    Arguments = $"/c psexec.exe \\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -nobanner powershell.exe /c Format-Hex \"{getFileLocationBox.Text}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc.Start();

            List<String> raw_lines = new List<string>(proc.StandardOutput.ReadToEnd().Split('\n'));
            List<String> lines = new List<string>();
            List<String> byte_lines = new List<string>();
            proc.Close();
            // get out the bad shit
            int i = 0;
            foreach (string line in raw_lines)
            {
                if (i <= 5 || i >= raw_lines.Count - 3)
                {
                    i++;
                    continue;
                }
                lines.Add(line);
                i++;
            }

            // Get only the bytes themselves
            foreach (string line in lines)
                byte_lines.Add(line.Substring(11, 47).Trim());
            string[] x = byte_lines.ToArray();
            // get the bytes
            List<byte> bytes = new List<byte>();
            foreach (string line in byte_lines)
                foreach (string bt in line.Split(' '))
                    bytes.Add((byte)int.Parse(bt, System.Globalization.NumberStyles.HexNumber));
            ByteArrayToFile(saveToBox.Text + "\\" + getFileLocationBox.Text.Split('\\').Last(), bytes.ToArray());

            var proc1 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start {saveToBox.Text}\\{getFileLocationBox.Text.Split('\\').Last()}",
                    UseShellExecute = false,
                    RedirectStandardOutput = false,

                    CreateNoWindow = true
                }
            };
            proc1.Start();
            proc1.Close();
        }

        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        private void chooseSaveBtn_Click(object sender, EventArgs e)
        {
            
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select file location and name";
                
                if (fbd.ShowDialog() == DialogResult.OK)
                    saveToBox.Text = fbd.SelectedPath;

            }
        }

        private void screenshotGoBtn_Click(object sender, EventArgs e)
        {
            screenWidthBox.Value = screenWidthBox.Value < 50 ? 1920 : screenWidthBox.Value;
            screenHeightBox.Value = screenHeightBox.Value < 50 ? 1920 : screenHeightBox.Value;
            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -i -accepteula nircmd.exe savescreenshot C:\\capture.png 0 0 {screenWidthBox.Value} {screenHeightBox.Value}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc2.Start();
            proc2.Close();
            
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c psexec.exe \\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -nobanner powershell.exe /c Format-Hex C:\\capture.png",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            proc.Start();
            var delIt = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c psexec.exe \\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -nobanner cmd.exe /c del C:\\capture.png",
                }
            };
            //delIt.Start();
            List<String> raw_lines = new List<string>(proc.StandardOutput.ReadToEnd().Split('\n'));
            List<String> lines = new List<string>();
            List<String> byte_lines = new List<string>();
            proc.Close();
            // get out the bad shit
            int i = 0;
            foreach (string line in raw_lines)
            {
                if (i <= 5 || i >= raw_lines.Count - 3)
                {
                    i++;
                    continue;
                }
                lines.Add(line);
                i++;
            }

            // Get only the bytes themselves
            foreach (string line in lines)
                byte_lines.Add(line.Substring(11, 47).Trim());
            string[] x = byte_lines.ToArray();
            // get the bytes
            List<byte> bytes = new List<byte>();
            foreach (string line in byte_lines)
                foreach (string bt in line.Split(' '))
                    bytes.Add((byte)int.Parse(bt, System.Globalization.NumberStyles.HexNumber));
            
            ByteArrayToFile(saveToBox.Text + "\\capture.png", bytes.ToArray());

            var startPic = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start {saveToBox.Text}\\capture.png",
                    UseShellExecute = false,
                    RedirectStandardOutput = false,

                    CreateNoWindow = true
                }
            };
            startPic.Start();
            startPic.Close();
        }
        
        private void remoteConsoleBtn_Click(object sender, EventArgs e)
        {
            RemoteConsole remoteConsoleForm = new RemoteConsole(this);
            remoteConsoleForm.Show();
            /*
            Process proc = new Process();

            proc.StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C echo Connecting To {ipBox.Text} & timeout /t 2 >nul & psexec.exe \\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -nobanner cmd.exe & cmd",
            };
            proc.Start();
            */
        }
    }
}
