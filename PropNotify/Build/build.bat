echo off

set MSTEST="C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\MSTest.exe"
set FOLDER="..\PropNotify\PropNotify\bin\Release\"
rem ************** Build ************** 
set /P BUILD=Do you want to build now [y/n]? 
if "%BUILD%"=="y" goto BUILD
goto END_BUILD

:BUILD
echo *** Building...
dotnet build ..\Propnotify.sln --configuration Release
if errorlevel 1 goto BUILD_FAIL

echo *** Running tests...
%MSTEST% /testcontainer: ..\PropNotify.Tests\bin\Release\PropNotify.Test.dll
if errorlevel 1 goto TEST_FAIL
:END_BUILD

rem ************** NuGet ************** 
set /P NUGET=Do you want to publish to NuGet now [y/n]? 
if /i "%NUGET%"=="y" goto NUGET
goto END

:NUGET
NOTEPAD PropNotify.nuspec
echo *** Creating NuGet package
xcopy Notify.nuspec %FOLDER%
mkdir %FOLDER%lib\net40\ 
move /Y %FOLDER%Notify.dll %FOLDER%lib\net40\
move /Y %FOLDER%Notify.xml %FOLDER%lib\net40\
nuget pack %FOLDER%Notify.nuspec
if errorlevel 1 goto PACK_FAIL

:VERSION
set /P VERSION=Enter version: 
if /i "%VERSION%"=="" goto VERSION
set PACKAGE=PropNotify.%VERSION%.nupkg
echo *** Publishing NuGet package...
nuget push %PACKAGE%
goto END

:BUILD_FAIL
echo *** BUILD FAILED ***
goto END

:TEST_FAIL
echo *** TEST FAILED ***
goto END

:PACK_FAIL
echo *** PACKING FAILED ***
goto END

:END