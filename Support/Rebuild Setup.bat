@echo off

pushd "%~dp0"

powershell -file ".\Rebuild Setup.ps1"

pause

popd
