FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
RUN apt update && apt install -y procps && apt install -y vim && rm -rf /var/lib/apt/lists/*
COPY Config/ et-server/Config/
COPY Logs/ et-server/Logs/
COPY publish/ et-server/publish/
WORKDIR et-server/publish/
VOLUME /et-server/Logs/
ENTRYPOINT ["dotnet", "App.dll"]