#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.15-bullseye-slim-arm32v7 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

WORKDIR /src
COPY ["box.api/*.csproj", "box.api/"]
COPY ["box.application/*.csproj", "box.application/"]
COPY ["box.infrastructure/*.csproj", "box.infrastructure/"]
COPY ["box.database/*.sqlproj", "box.database/"]

RUN dotnet restore "box.api/box.api.csproj"

COPY . .
WORKDIR "/src/box.api"
#RUN dotnet build "box.api.csproj" -c Release -o /app/build
RUN dotnet publish "box.api.csproj" -c Release --runtime linux-arm -o /app/publish



FROM mcr.microsoft.com/dotnet/runtime:6.0.15-alpine3.17-arm32v7 AS final
WORKDIR /app

EXPOSE 80
EXPOSE 443

ARG ConnStr=ConnStrNotSet
ARG ApiKey=KEY

ENV ConnStr=${ConnStr}
ENV X-API-KEY=${ApiKey}

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "box.api.dll"]