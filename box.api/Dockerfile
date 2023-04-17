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
RUN dotnet publish "box.api.csproj" -c Release -o /app/publish



FROM mcr.microsoft.com/dotnet/aspnet:6.0.16-alpine3.17-amd64 AS final

WORKDIR /app

RUN apk add --no-cache icu-libs

EXPOSE 80
EXPOSE 443

ARG ConnStr=ConnStrNotSet
ARG ApiKey=KEY

ENV ASPNETCORE_URLS=http://*:5001
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ConnStr=${ConnStr}
ENV X-API-KEY=${ApiKey}

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "box.api.dll"]