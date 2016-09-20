MSBuild.exe dir-date.sln /p:Configuration=Debug /p:Platform="Any CPU"
if errorlevel 1 goto :eof
bin\Debug\dir-date %*
