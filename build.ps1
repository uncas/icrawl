"This build script is still incomplete... please use build.cmd and WebTester.build for 'real' builds."

.\GetTools.ps1
Import-Module .\packages\psake.4.0.1.0\tools\psake.psm1
.\packages\psake.4.0.1.0\tools\psake.cmd .\scripts\build.ps1