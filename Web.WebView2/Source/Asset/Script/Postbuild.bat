echo -------------------------------------------------------------------------------
echo    App.Web.WebView2     ::     Post-buid - Copy DLL
robocopy "..\..\..\Assembly\web " ".\ " WebView2Loader.dll /XO
robocopy ".\ " "..\..\..\Bin " *.dll *.pdb *.xml /XO
if %errorlevel% leq 4 echo App.Web.WebView2 Post-buid ExitCode: %errorlevel%&echo:& exit 0 else exit %errorlevel%