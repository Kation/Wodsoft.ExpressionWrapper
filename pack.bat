@echo off
set s_version=
set /p s_version=«Î ‰»Î∞Ê±æ∫≈£∫
if "%s_version%" neq "" set "s_version=--version-suffix %s_version%"
if not exist build md build
cd build
del *.nupkg
cd..
dotnet pack src\Wodsoft.ExpressionWrapper --output ..\..\build --include-source %s_version%
pause