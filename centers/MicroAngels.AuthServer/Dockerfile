#FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
#WORKDIR /app
#EXPOSE 80
#
FROM microsoft/dotnet:2.1-sdk
WORKDIR /app
COPY ./bin/publish /app
ENV ASPNETCORE_ENVIRONMENT "Production"
ENV ASPNETCORE_URLS http://*:2012
ENTRYPOINT ["dotnet", "MicroAngels.AuthServer.dll"]

#
#
#FROM microsoft/dotnet:2.1-sdk AS build
#WORKDIR /src
#COPY ["centers/MicroAngels.AuthServer/MicroAngels.AuthServer.csproj", "centers/MicroAngels.AuthServer/"]
#COPY ["src/MicroAngels.IdentityServer/MicroAngels.IdentityServer.csproj", "src/MicroAngels.IdentityServer/"]
#COPY ["src/MicroAngels.Core/MicroAngels.Core.csproj", "src/MicroAngels.Core/"]
#COPY . .
#ENV ASPNETCORE_ENVIRONMENT "Production"
#
#WORKDIR "/src/centers/MicroAngels.AuthServer"
#RUN dotnet build "MicroAngels.AuthServer.csproj" 
#FROM build AS publish
#RUN dotnet publish "MicroAngels.AuthServer" -c Release -o /app
#
#WORKDIR /app
#EXPOSE 2012
#ENTRYPOINT ["dotnet","MicroAngels.AuthServer.dll"]