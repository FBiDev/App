echo -------------------------------------------------------------------------------
echo    App.Core     ::     Post-buid - Copy DLL

robocopy ".\ " "..\..\..\Bin " *.dll *.pdb *.xml /XO

set ConfigurationName=%1
if %ConfigurationName% == Release (
robocopy ".\ " "..\..\..\Bin " /PURGE /IF *.pdb *.xml )

if %errorlevel% leq 4 echo App.Core Post-buid ExitCode: %errorlevel%&echo:& exit 0 else exit %errorlevel%