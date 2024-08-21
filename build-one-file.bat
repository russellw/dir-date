rem This does actually compile the program to a single exe
rem but that exe is 66 megabytes
dotnet publish  /p:Configuration=Release /p:Platform="Any CPU" -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
