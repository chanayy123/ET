FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
COPY Config/ et-server/Config/
COPY Logs/ et-server/Logs/
COPY publish/ et-server/publish/
WORKDIR et-server/publish/
VOLUME /et-server/Logs/
ENTRYPOINT ["dotnet", "App.dll"]