R3p.BDO
=======

.. image:: https://img.shields.io/github/issues/r3peat/R3p.BDO.svg
  :alt: Issues on Github
  :target: https://github.com/r3peat/R3p.BDO/issues

.. image:: https://img.shields.io/github/issues-pr/r3peat/R3p.BDO.svg
  :alt: Pull Request opened on Github
  :target: https://github.com/r3peat/R3p.BDO/issues

.. image:: https://img.shields.io/github/release/r3peat/R3p.BDO.svg
  :alt: Release version on Github
  :target: https://github.com/r3peat/R3p.BDO/releases/latest

.. image:: https://img.shields.io/github/release-date/r3peat/R3p.BDO.svg
  :alt: Release date on Github
  :target: https://github.com/r3peat/R3p.BDO/releases/latest

+--------------+--------------------------+---------------------------+---------------------------+--------------------------+--------------------------+--------------------------+
| Branch name  | R3p.bdo                  | R3p.bdo.GUIloader         | R3p.bdo.settings          | R3p.bdo.ui               | R3p.injector             | x64fw2                   |
+==============+==========================+===========================+===========================+==========================+==========================+==========================+
| master       | TODO: appveyor SVG       | TODO: appveyor SVG        | TODO: appveyor SVG        | TODO: appveyor SVG       | TODO: appveyor SVG       | TODO: appveyor SVG       |
+--------------+--------------------------+---------------------------+---------------------------+--------------------------+--------------------------+--------------------------+


Description
-----------

I decided to publish the source of this so people can learn from it or revamp it.
This project was created long long ago so please don't judge me cause of design flaws and performance issues. Just had no time to proper update it.
The Bot/Hack is a combination of external C# and an internal C++ dll, they communicate through a named pipe.
All dependend projects + dll project + loader project are inside "Imports" folder.


How to compile sources?
-----------------------

- Recreate classes with ftp credentials because are removed from **r3p.bdo.testconsole** + **r3p.bdo.guiloader**
- To make this work use ur **own ftp** with ur **own credentials** or **just disable these features**


!Required NuGet Packages!
~~~~~~~~~~~~~~~~~~~~~~~~~

- FluentFTP
- Microsoft.WindowsAPICodePack.Core
- Microsoft.WindowsAPICodePack.Shell
- SharpDX
- SharpDX.Direct2D1
- SharpDX.DXGI
- SharpDX.Mathematics


Compiled sources
----------------

- [ ] Download v760 : **TODO**
- [x] _`Download v759`: https://mega.nz/#!pfxChYKL!aq0DobI13uJjyAcONapmwUZnRiJfEU9YJE5lSZL6zzc
- [x] _`Download v753`: https://mega.nz/#!lHJEyRrS!IluBtUDZgEJ1LXwhWb3svGzC7vyU-HAsxDBlIUHPXRA



.. |r3pbdo_master_lin| image:: https://travis-ci.org/r3peat/R3p.BDO.svg?branch=master
.. |r3pbdo_master_win| image:: https://ci.appveyor.com/api/projects/status/f4orjhi6vjgsxxq9/branch/master?svg=true
