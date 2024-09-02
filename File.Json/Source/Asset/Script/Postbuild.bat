echo -------------------------------------------------------------------------------
echo    App.File.Json     ::     Post-buid - Copy DLL

robocopy ".\ " "..\..\..\Bin " *.dll *.pdb *.xml /XO

if %errorlevel% leq 4 echo App.File.Json Post-buid ExitCode: %errorlevel%&echo:& exit 0 else exit %errorlevel%