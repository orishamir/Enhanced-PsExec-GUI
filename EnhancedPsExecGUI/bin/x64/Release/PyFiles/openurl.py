import sys
import subprocess
import time
import os

# Usage: openurl -ip [ip] -u [username] -p [password] -url [url]
print("ahahasd123")
if len(sys.argv) < 7:
    exit("Not Enough Arguments.")
incognito = False
new_window = False
invisible = False
tabs = 1

delay_before = 0
delay_between = 0
url = "*://*/*"
for i, arg in enumerate(sys.argv):
    if arg.lower() == '-i' or arg.lower() == '-incognito':
        incognito = True
    if arg.lower() == "-invis" or arg.lower() == "-inv" or arg.lower() == "-invisible":
        invisible = True
    if arg.lower() == "-new" or arg.lower() == '-newwindow' or arg.lower() == "-nw":
        new_window = True
    if arg.lower() == '-tabs' or arg.lower() == '-n':
        tabs = int(sys.argv[i+1])

    if arg.lower() == "-dbf" or arg.lower() == "-delaybefore":
        delay_before = int(sys.argv[i+1])/1000
    if arg.lower() == "-dbt" or arg.lower() == "-delaybetween":
        delay_between = int(sys.argv[i + 1]) / 1000
    # check for username password and ip
    if arg.lower() == '-ip':
        ip = sys.argv[i+1]
    if arg.lower() == '-u':
        username = sys.argv[i+1]
    if arg.lower() == '-url':
        url = sys.argv[i+1]
    if arg.lower() == '-p' or arg.lower() == '-pass' or arg.lower() == '-password':
        password = sys.argv[i+1]



adminsetting = "-s"
dontwaitforterminate = "-d"
interactive = "-i"
accept_eula = "-accepteula"


def openurl(fromFile="fileName.txt", delimiter=' '):

    """

    URL --- This is the URL to be opened in the remote machine. If `fromFile` parameter is used, it must be: `'*://*/*'`, its default

    fromFile --- This parameter is used to take A text file and get every URL and its shotcut name.
    See more: https://github.com/orishamir/Epsexec/blob/master/fromFile.md

    delimiter --- This is only if you also specified `fromFile` - How to seperate each name,url

    tabs --- This parameter is responsible for the amount of tabs to open on the remote machine. (Default=1)

    delayBeforeOpening --- This parameter decides how much time in millisecond the program should pause before starting the operation. (Default=100)

    delayBetweenTabs --- This parameter decides how much time in millisecond the program should pause BETWEEN every time it opens A new tab.

    new_window --- This parameter decides whether or not to open the tab(s) in new window each time. (Default=False)

    incognito --- This parameter decides if the tab(s) would be opened in Incognito mode. (Default=False)

    invisible --- This parameter decides if the tab(s) would be opened invisibly, and not interactive, so the user would not notice its opened, unless the window plays sound (Default=False).
    """
    global ip
    global username
    global password
    global tabs

    global new_window
    global url
    global incognito
    global invisible

    global delay_before
    global delay_between

    global adminsetting
    global interactive
    global accept_eula
    global dontwaitforterminate
    # set minimum requirements for delay before opening and delay between tab

    def get_installation_folder():
        p = subprocess.Popen(
            f'psexec \\\\{ip} -u {username} -p {password} {accept_eula} {adminsetting} cmd.exe /c cd "c:\\Program Files (x86)\\Google\\Chrome\\Application" & where chrome.exe',
            stdin=subprocess.PIPE, stdout=subprocess.PIPE, stderr=subprocess.PIPE)

        p.communicate(b"input data that is passed to subprocess' stdin")
        return_code = p.returncode
        if return_code == 0:
            # Its in program files x86

            Installation_location = 'c:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe'

        else:
            p = subprocess.Popen(
                f'psexec \\\\{ip} -u {username} -p {password} {accept_eula} {adminsetting} cmd.exe /c cd "%userprofile%\\AppData\\Local\\Google\\Chrome\\Application" & where chrome.exe',
                stdin=subprocess.PIPE, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
            p.communicate(b"input data that is passed to subprocess' stdin")
            return_code = p.returncode
            if return_code == 0:
                user_profile = "C:\\users\\{0}".format(username)
                Installation_location = user_profile + '\\AppData\\Local\\Google\\Chrome\\Application\\chrome.exe'

            else:
                # Its in program files x64
                Installation_location = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
        return Installation_location

    # get incognito value
    if incognito:
        incognito = " -incognito"
    else:
        incognito = ""

    # --new-window to get new tab
    if new_window:
        new_window = "--new-window"
    else:
        new_window = ""

    # IMPORT FROM FILE ################################3
    if fromFile != "fileName.txt" or url == "*://*/*":
        # get user text file name (fromFile)
        # save into list using a pre-made config structure
        # output to the user the things
        # get user's choice

        with open(fromFile, 'r', encoding="UTF-8-sig") as file:
            urls = {}
            can_start = False
            for line in file.readlines():
                if "urls:" in line or "url:" in line:
                    can_start = True
                    continue
                if "endurl" in line or line == 'config:':
                    can_start = False
                    break
                if can_start:
                    # get name and URL to assign to the dictionary. split them by space and get only the string itself
                    # without special characters like \n or \t
                    line = line.split(delimiter)
                    line = filter(lambda x: x != '', line)

                    name, url = line
                    name = name.replace(' ', '')
                    name = name.replace('\t', '')
                    url = url.replace(' ', '')
                    url = url.replace('\n', '')

                    urls[name] = url
        # ended getting values from file

        ##########################################
        # what does the USER want?
        user_index = 1

        for shortName, url in urls.items():
            print(f'      {user_index}. {shortName} {"":.^{40 - len(shortName)}} {url}')
            user_index += 1

        # In python  3.8 - change this to: while (user_index := input("Choice: ") (not in urls.keys() and type(
        # user_index) != int) or (type(user_index) == int and user_index > len(urls.keys())):

        user_index = input("\n    Choice: ")

        # if the user picked the name not by index number but from shortcut name then yes
        if user_index in urls.keys():
            url = urls[user_index]
        else:
            if user_index.isalpha():
                raise ValueError("URL Shortcut does not exist.")
            # get the list of the urls and get the user's index minus 1 cuz it starts at 0.
            elif int(user_index) > len(urls.keys()):
                raise IndexError("URL index does not exist.")
            url = list(urls.values())[int(user_index) - 1]

    # sleep before opening tabs
    time.sleep(delay_before)

    if invisible:
        # if user wants invisible to be True:
        if delay_between == 500 and new_window != "--new-window":
            subprocess.call(f"psexec \\\\{ip} -u {username} -p {password} cmd /c start " + f" {url} "*tabs + f" {new_window} {incognito} ")
        for tab in range(1, tabs + 1):
            # make invisible somehow...
            subprocess.call(f"psexec \\\\{ip} -u {username} -p {password} {accept_eula} cmd.exe /c start chrome {url} {new_window} {incognito}")
            time.sleep(delay_between)
    else:
        # get chrome installation location
        try:
            installation_location = globals()['installation_location']
        except KeyError:
            globals()['installation_location'] = get_installation_folder()

        installation_location = globals()['installation_location']
        # if user DOES NOT WANT invisible:
        # if we can do it in one line, do it
        if delay_between == 10 and new_window != "--new-window":
            subprocess.call(f"psexec \\\\{ip} -u {username} -p {password} {interactive} {dontwaitforterminate} {accept_eula} \"{installation_location}\"" + f" {url} " * tabs + f" {new_window} {incognito} {accept_eula}")
        # else:
        for tab in range(1, tabs + 1):
            # make start visible
            subprocess.call(f"psexec \\\\{ip} -u {username} -p {password} {interactive} {dontwaitforterminate} {accept_eula} \"{installation_location}\" {url} {new_window} {incognito}")
            time.sleep(delay_between)
    os.system("pause")


openurl()
