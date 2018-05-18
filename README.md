# R3p.BDO

I decided to publish the source of this so people can learn from it or revamp it.

This project was created long long ago so please don't judge me cause of design flaws and performance issues. Just had no time to proper update it.

The Bot/Hack is a combination of external C# and an internal C++ dll, they communicate through a named pipe.

All dependend projects + dll project + loader project are inside "Imports" folder.

!Important Note!
- class with ftp credentials are removed from r3p.bdo.testconsole + r3p.bdo.guiloader
-> to make this work use ur own ftp with ur own credentials or just disable these features

!Required NuGet Packages!
- FluentFTP
- Microsoft.WindowsAPICodePack.Core
- Microsoft.WindowsAPICodePack.Shell
- SharpDX
- SharpDX.Direct2D1
- SharpDX.DXGI
- SharpDX.Mathematics

!v753 x64 BDO client dump for analyze purposes!

https://mega.nz/#!lHJEyRrS!IluBtUDZgEJ1LXwhWb3svGzC7vyU-HAsxDBlIUHPXRA
