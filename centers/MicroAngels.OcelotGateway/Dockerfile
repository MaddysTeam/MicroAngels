FROM microsoft/dotnet:2.1-sdk
WORKDIR /app
COPY ./bin/publish /app
ENV ASPNETCORE_ENVIRONMENT "Production"
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "MicroAngels.OcelotGateway.dll"]
