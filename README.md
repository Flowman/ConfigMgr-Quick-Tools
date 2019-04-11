
# ConfigMgr Quick Tools

ConfigMgr Quick Tools is a compilation of tools to simplify your daily usage of ConfigMgr. Guess you have been in the same boat as me when you think some implementations are a bit wonky and you want to work faster. 

There is a bunch of excellent tools out there, but most of them are written in PowerShell and are slow compared to c#. I tried to create something that looks and feel like it would be a part of ConfigMgr.

Improvement to the console:
 - Driver Manager (grabber, importer)
	 - Dell Driver Pack Downloader
 - Extended Device properties (update compliance, dell warranty, client health, collection membership)
 - Collection management (add resources from list)
 - Client Actions (quicker than right click tools)
 - LAPS password
 - Software Update (clean-up groups, phased deployments)

## Install Quick Tools

Download the latest release, Unzip and run the "Install-QuickTools.ps1" with admin rights.

I will recreate the MSI installer one day when I figure out how to create one.

## How to build

- Install ConfigMgr Console
- Install Visual Studio
- Load up the project and check the all references are OK
- Build 

## Wiki

See the [Wiki](https://github.com/Flowman/ConfigMgr-Quick-Tools/wiki) for documentation.

## How to contribute

- Report bugs
- If you want to contribute fork the code and create a pull request

## License

[MIT](http://opensource.org/licenses/MIT)

Copyright (c) Peter Szalatnay