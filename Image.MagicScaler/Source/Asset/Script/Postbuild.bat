echo -------------------------------------------------------------------------------
echo    App.Image.MagicScaler     ::     Post-buid - Copy DLL

set ConfigurationName=%1
if %ConfigurationName% == Release (
robocopy ".\ " "..\..\..\Bin " *.dll /XO
) else (
robocopy ".\ " "..\..\..\Bin " *.dll *.pdb *.xml /XO )

if %errorlevel% leq 4 echo App.Image.MagicScaler Post-buid ExitCode: %errorlevel%&echo:& exit 0 else exit %errorlevel%