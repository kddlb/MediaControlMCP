dotnet publish MediaControlMCP.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o "$env:USERPROFILE\.local\bin"
