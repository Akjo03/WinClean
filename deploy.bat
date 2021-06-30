@echo off
echo Deplyoing and publishing WinClean... please wait!

set runtime=win-x64
set version=0.1.0

dotnet publish WinClean.csproj -c Release --framework netcoreapp3.1 --runtime %runtime% --nologo -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true -v q --version-suffix %version%
if errorlevel 1 goto error
echo Finished deploying WinClean!
goto end
:error
echo [91mError on deploying WinClean![0m
goto end
:end
pause.