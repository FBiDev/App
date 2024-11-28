@echo off
setlocal EnableDelayedExpansion EnableExtensions

set "EventType=Pre-build"
set "EventName=Clean Bin"

set "ProjectName=%1"
set "SolutionDir=%2"
set "TargetDir=%3"

echo ===============================================================================
echo    Project      ::     %ProjectName%
echo    Begin-event  ::     %EventType% -^> %EventName%
echo ===============================================================================

robocopy "%TargetDir% " "%SolutionDir%Bin " /PURGE

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