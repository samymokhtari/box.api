FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

WORKDIR /src

COPY ["box.api/*.csproj", "box.api/"]
COPY ["box.application/*.csproj", "box.application/"]
COPY ["box.infrastructure/*.csproj", "box.infrastructure/"]
COPY ["box.database/*.sqlproj", "box.database/"]

RUN dotnet restore "box.api/box.api.csproj"
RUN dotnet restore "box.application/box.application.csproj"
RUN dotnet restore "box.infrastructure/box.infrastructure.csproj"

COPY . .
WORKDIR "/src/box.api"
RUN dotnet publish "box.api.csproj" -c Release -o /app/publish



FROM mcr.microsoft.com/dotnet/aspnet:6.0.16-alpine3.17-amd64 AS final

WORKDIR /app

RUN apk add --no-cache icu-libs

ENV ASPNETCORE_URLS=http://*:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "box.api.dll"]