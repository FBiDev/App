echo -------------------------------------------------------------------------------
echo    App.File.Report     ::     Post-buid - Copy DLL

robocopy ".\ " "..\..\..\Bin " *.dll *.pdb *.xml /XO

if %errorlevel% leq 4 echo App.File.Report Post-buid ExitCode: %errorlevel%&echo:& exit 0 else exit %errorlevel%