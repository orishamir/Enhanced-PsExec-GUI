from time import sleep
import sys
from subprocess import call

# Usage: closechrome -ip [ip] -u [username] -p [password] -delay <amount>

if len(sys.argv) < 6:
    sys.exit("Usage: closechrome -ip [ip] -u [username] -p [password] -delay <amount>")
ip = ''
username = ''
password = ''

delay_before = 0
for i, arg in enumerate(sys.argv):
    if arg.lower() == "-ip":
        ip = sys.argv[i+1]
    if arg.lower() == "-u":
        username = sys.argv[i+1]
    if arg.lower() == "-p":
        password = sys.argv[i+1]

    if arg.lower() == "-delay" or arg.lower() == "-d":
        delay_before = int(sys.argv[i+1])/1000

adminsetting = "-s"
dontwaitforterminate = "-d"
interactive = "-i"
accept_eula = "-accepteula"


def close_chrome():
    global adminsetting
    global dontwaitforterminate
    global interactive
    global accept_eula

    global ip
    global username
    global password

    global delay_before
    print(ip, username, password, delay_before)
    sleep(delay_before)
    call(f"psexec \\\\{ip} -u {username} -p {password} {adminsetting} {accept_eula} cmd.exe /c taskkill /F /IM chrome.exe /t")


close_chrome()
