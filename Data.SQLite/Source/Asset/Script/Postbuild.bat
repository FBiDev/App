echo -------------------------------------------------------------------------------
echo    App.Data.SQLite     ::     Post-buid - Copy DLL

robocopy "..\..\..\Assembly\data " ".\ " SQLite.Interop.dll /XO
robocopy ".\ " "..\..\..\Bin " *.dll *.pdb *.xml /XO

if %errorlevel% leq 4 echo App.Data.SQLite Post-buid ExitCode: %errorlevel%&echo:& exit 0 else exit %errorlevel%