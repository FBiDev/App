echo -------------------------------------------------------------------------------
echo    App.Cohab    ::     Post-buid - Copy DLL
robocopy ".\ " "..\..\..\Bin " *.dll *.pdb *.xml /XO
if %errorlevel% leq 4 echo App.Cohab Post-buid ExitCode: %errorlevel%&echo:& exit 0 else exit %errorlevel%