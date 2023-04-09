#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.15-alpine3.17-arm64v8 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0.407-alpine3.17-arm64v8 AS build
WORKDIR /src
COPY ["box.api/*.csproj", "box.api/"]
COPY ["box.application/*.csproj", "box.application/"]
COPY ["box.infrastructure/*.csproj", "box.infrastructure/"]
COPY ["box.database/*.sqlproj", "box.database/"]

COPY . .
WORKDIR "/src/box.api"
RUN dotnet build "box.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "box.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

ARG ConnStr=ConnStrNotSet
ARG ApiKey=KEY

ENV ConnStr=${ConnStr}
ENV X-API-KEY=${ApiKey}

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "box.api.dll"]