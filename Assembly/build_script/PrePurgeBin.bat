@echo off
setlocal EnableDelayedExpansion EnableExtensions

set "EventType=Pre-build"
set "EventName=Clean Bin"

set "ProjectName=%1"
set "SolutionDir=%2"

echo ===============================================================================
echo    Project      ::     %ProjectName%
echo    Event        ::     Begin %EventType% -^> %EventName%
echo ===============================================================================

rd /s /q "%SolutionDir%Bin\"

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