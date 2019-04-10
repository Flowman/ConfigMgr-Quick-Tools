<#
 _______  _______  __   __  _______  ______    ___      _______  __    _  ______  
|       ||       ||  | |  ||       ||    _ |  |   |    |   _   ||  |  | ||      | 
|   _   ||_     _||  |_|  ||    ___||   | ||  |   |    |  |_|  ||   |_| ||  _    |
|  | |  |  |   |  |       ||   |___ |   |_||_ |   |    |       ||       || | |   |
|  |_|  |  |   |  |       ||    ___||    __  ||   |___ |       ||  _    || |_|   |
|       |  |   |  |   _   ||   |___ |   |  | ||       ||   _   || | |   ||       |
|_______|  |___|  |__| |__||_______||___|  |_||_______||__| |__||_|  |__||______| 

    .SYNOPSIS
        Install ConfigMgr Quick Tools
    .DESCRIPTION

    .NOTES
        This program is free software: you can redistribute it and/or modify
        it under the terms of the GNU General Public License as published by
        the Free Software Foundation, either version 3 of the License, or
        (at your option) any later version.

        This program is distributed in the hope that it will be useful,
        but WITHOUT ANY WARRANTY; without even the implied warranty of
        MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        GNU General Public License for more details.

        You should have received a copy of the GNU General Public License
        along with this program.  If not, see <http://www.gnu.org/licenses/>.

        Author: Peter Szalatnay
        Version 1.0.0
            - Initial Creation

    .EXAMPLE
        Install-QuickTools
#> 

$consolePath = Get-ItemPropertyValue "HKLM:\SOFTWARE\WOW6432Node\Microsoft\ConfigMgr10\Setup" -Name "UI Installation Directory"

$MSIUninstallArguments = @(
    "/x"
    "{08FF2A6E-2C00-418E-B18C-48D69D13DE09}"
    "/q"
)
Start-Process "msiexec.exe" -ArgumentList $MSIUninstallArguments -Wait -NoNewWindow

# update all MSG xml file with the correct statview.exe
$files = Get-Childitem "$PSScriptRoot\Extensions\Actions" -recurse -force | Where-Object { -not $_.psisdirectory } | Where-Object { $_.name.endswith('Msg.xml') } 
foreach ($file in $files){

    [xml]$xmlDoc = Get-Content $file.FullName

    $xmlDoc.ActionDescription.Executable.FilePath = "$($consolePath)bin\i386\statview.exe"

    $xmlDoc.Save($file.FullName)
}

# copy quick tools to console bin dir
Copy-Item -Path "$PSScriptRoot\Build\ConfigMgr.QuickTools*.dll"  -Destination "$($consolePath)bin\"
Copy-Item -Path "$PSScriptRoot\Build\ByteSize.dll" -Destination "$($consolePath)bin\"
Copy-Item -Path "$PSScriptRoot\Build\INIFileParser.dll" -Destination "$($consolePath)bin\"
Copy-Item -Path "$PSScriptRoot\Build\Microsoft.Deployment.Compression.Cab.dll" -Destination "$($consolePath)bin\"
Copy-Item -Path "$PSScriptRoot\Build\Microsoft.Deployment.Compression.dll" -Destination "$($consolePath)bin\"

Copy-Item -Path "$PSScriptRoot\Extensions"  -Destination "$($consolePath)XmlStorage\" -Recurse -Force
