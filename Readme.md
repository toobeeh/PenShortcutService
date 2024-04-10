# Pen Shortcut Service
Quick and dirty tool to disable my touchscreen with the pen shortcut button.  
When installed, this toggles the touchscreen with a chosen pen shortcut button.

## Installation:
- Build the project
- Register PenShortcutService/bin/Debug/dotnet8.0/PenShortcutService.exe as service (eg with powershell)
- Go to the windows ink settings and set the pen shortcut to "program"
- Choose the PenShortcutClient/bin/Debug/dotnet8.0/PenShortcutClient.exe as program

Each time the program gets run (pen button is clicked), the service toggles the touchscreen.