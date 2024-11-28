@echo off
setlocal EnableDelayedExpansion EnableExtensions

set "EventType=Post-build"
set "EventName=Copy DLL ^& WebView2Loader"

set "ProjectName=%1"
set "SolutionDir=%2"
set "TargetDir=%3"
set "ConfigurationName=%4"

echo ===============================================================================
echo    Project      ::     %ProjectName%
echo    Begin-event  ::     %EventType% -^> %EventName%
echo ===============================================================================

robocopy "%SolutionDir%Assembly\web " "%TargetDir% " WebView2Loader.dll /XO


if %ConfigurationName% == Release (
	robocopy "%TargetDir% " "%SolutionDir%Bin " *.dll /XO
) else (
	robocopy "%TargetDir% " "%SolutionDir%Bin " *.dll *.pdb *.xml /XO
)

echo ===============================================================================
echo    Project      ::     %ProjectName%
echo    End-event    ::     %EventType% -^> %EventName%
echo    Result       ::     ExitCode: %errorlevel%
echo ===============================================================================
echo:

if %errorlevel% leq 4 (
	exit 0
) else (
	exit %errorlevel%
)