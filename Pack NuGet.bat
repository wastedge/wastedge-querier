@echo off

pushd "%~dp0"

cd WastedgeQuerier
..\Libraries\NuGet\NuGet.exe pack -NoPackageAnalysis -Tool -Prop Configuration=Release

pause

popd
