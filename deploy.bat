@echo off
echo Deplyoing and publishing WinClean... please wait!

set runtime=win-x64
set version=0.0.0

dotnet publish -c Release --framework netcoreapp3.1 --runtime %runtime% --nologo -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true -v q --version-suffix %version%
echo Finished deploying WinClean!
pause.
exit