echo -------------------------------------------------------------------------------
echo    App.Image.MagicScaler     ::     Post-buid - Copy DLL
robocopy ".\ " "..\..\..\Bin " *.dll *.pdb *.xml /XO
if %errorlevel% leq 4 echo App.Image.MagicScaler Post-buid ExitCode: %errorlevel%&echo:& exit 0 else exit %errorlevel%