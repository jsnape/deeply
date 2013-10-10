@echo off
setlocal

set ROOTDIR=%~dp0
pushd %ROOTDIR%

msbuild .\build\build.proj

popd

endlocal