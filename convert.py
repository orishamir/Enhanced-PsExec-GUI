"""
I don't use this anymore since I just rewrote everything in C#
instead of in Py and convert to exe and some not nice stuff...
but i'll leave it here.


"""

import os
import shutil
"""Convert all .py files to .exe files"""

# remove already existing previous compiled
for f in os.listdir(os.getcwd()):
    if f.endswith(".exe") and f != "EnhancedPsExecGUI.exe":
        os.remove(f)

for f in os.listdir(os.getcwd()):
    if f.endswith('.py') and f != "convert.py":
        os.system(f"pyinstaller -y -F --onefile {f}")
        try:
            os.remove(f'{os.getcwd()}\\{f[:-3]+".spec"}')
        except OSError as e:
            print(f"Ignoring error: \"{str(e)[13:]}\"")
# move dist/files to root directory.
for f in os.listdir(os.getcwd()+'\\dist\\'):
    shutil.move(f"{os.getcwd()}\\dist\\{f}", f"{os.getcwd()}\\")

shutil.rmtree(f'{os.getcwd()}\\build')
os.rmdir(f'{os.getcwd()}\\dist')
try:
    os.remove(f'{os.getcwd()}\\EnhancedPsExecGUI.exe.config')
    os.remove(f'{os.getcwd()}\\EnhancedPsExecGUI.pdb')
except OSError as e:
    print(f"Ignoring Error: \"{str(e)[13:]}\"")
