# [For The Python Version And A More Detailed Explanation Click Here](https://pypi.org/project/Enhanced-PsExec/)

### About
epsexec (Enhanced psexec) uses [Microsoft's Sysinternals PsExec](https://docs.microsoft.com/en-us/sysinternals/downloads/psexec) utility that uses SMB to execute programs on remote systems.
PsExec is a light-weight telnet replacement.    
If you find any bugs, PLEASE report to ***`EpsexecNoReply@gmail.com`***   

## Installation
Go To [here](https://github.com/orishamir/Enhanced-PsExec-GUI/releases)   
This will download an installer that would configure everything    


## Requirements
**Attacker Machine:**   
1) You MUST have [psexec install](https://docs.microsoft.com/en-us/sysinternals/downloads/psexec)ed   
    (The installer installed it for you)
   **If you have A 32-bit installation, install psexec to `C:\windows\sysWOW64`.**   
   Else, to `C:\windows\system32`   

**The Remote PC:**   
The remote pc (The pc that you are attacking) has very few requirements;
1) SMBv2 needs to be up and running on the Windows port. Run this CMD script on the remote computer:
`powershell.exe Set-SmbServerConfiguration -EnableSMB2Protocol $true`
2) The ADMIN$ share to be enabled with read/write access of the user configured.   
   Unless the machine already has an administrator user with password, I recommend making Another user that is administrator.   
   CMD:   
`net user /add usernameToHack passToBeUsed`   
To enable administrator:   
`net localgroup administrators usernameToHack /add`    

3) You'll need to add A registry key.   
This is because UAC is set up to deny connections like this, so you will get an `ACCESS_IS_DENIED` error when attempting to connect.   
Fix: Open CMD as administrator and run:   
`reg add HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\system /v LocalAccountTokenFilterPolicy /t REG_DWORD /d 1 /f`   

4) RECOMMENDED: Disable firewall on the remote machine.   
This will allow for a faster experience while connecting.   
~~There is also A tab for this~~, so you don't have to go to the remote PC NOW.   
Or, Open CMD with administrator privileges on the remote machine and run:   
`netsh advfirewall firewall set rule name="File and Printer Sharing (SMB-In)" dir=in new enable=Yes`   
Or, you can just disable the firewall entirely from administrator CMD:   
`netsh advfirewall set allprofiles state off`

5) Restart the system.   

## NirCMD
[NirCMD](https://www.nirsoft.net/utils/nircmd.html) is A windows command-line utility that allows you to do useful tasks without displaying any user interface.   
Unfortunately, NirCMD is NOT installed by default on windows systems.   
Thats why there is a button in the "Fun" tab, that automatically downloads NirCMD on the remote PC for you.   
Nircmd is required to be installed on the **remote** machine for all of the functions in:       
1. The misc tab
2. Screenshot grabber
3. The sound tab
4. "Set Process Volume", "Mute process", "Unmute process" in the Process Control tab