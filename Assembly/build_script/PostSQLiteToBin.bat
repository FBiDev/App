@echo off
setlocal EnableDelayedExpansion EnableExtensions

set "EventType=Post-build"
set "EventName=Copy DLL ^& SQLite.Interop"

set "ProjectName=%1"
set "TargetDir=%2"
set "SolutionDir=%3"
set "ConfigurationName=%4"

echo ===============================================================================
echo    Project      ::     %ProjectName%
echo    Event        ::     Begin %EventType% -^> %EventName%
echo ===============================================================================

:: robocopy "%SolutionDir%Assembly\data\x86 " "%TargetDir%x86 " SQLite.Interop.dll /IS
:: robocopy "%SolutionDir%Assembly\data\x64 " "%TargetDir%x64 " SQLite.Interop.dll /IS

if %ConfigurationName% == Release (
	robocopy "%TargetDir% " "%SolutionDir%Bin " *.dll /E /IS
) else (
	robocopy "%TargetDir% " "%SolutionDir%Bin " *.dll *.pdb *.xml /E /IS
)

echo ===============================================================================
echo    Project      ::     %ProjectName%
echo    Event        ::     End %EventType% -^> %EventName%
echo    Result       ::     ExitCode: %errorlevel%
echo ===============================================================================
echo:

if %errorlevel% leq 4 (
	exit 0
) else (
	exit %errorlevel%
)