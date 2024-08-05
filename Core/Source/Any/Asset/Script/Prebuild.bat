echo -------------------------------------------------------------------------------
echo    App.Core     ::     Pre-buid - Clean Bin
robocopy ".\ " "..\..\..\Bin " /PURGE /XF App.Core.*
if %errorlevel% leq 4 echo App.Core Pre-buid ExitCode: %errorlevel%&echo:& exit 0 else exit %errorlevel%