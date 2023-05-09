# Box

## Description

Here is a project which is an API to store some file on a server. It's a simple API developed with ASP.NET Core.

You can find below the architecture designed which represent all resources used and interactions in this project
![Architecture](docs/architecture.png?raw=true "Architecture")

## Installation

Install .NET 6.0. https://dotnet.microsoft.com/en-us/download/dotnet/6.0

Install Docker if you want to use it. https://docs.docker.com/get-docker/

## Get started

```sh
docker build -t box .
docker run -d -p 5000:8080 --name box box
```

## Environment Variables

To set a database, start a Visual Studio CLI and run this command:

```sh
dotnet user-secrets set "ConnectionStrings:ConnStr" "Server=SERVERADDRESS;Database=DATABASE;User ID=USERID;Password=PASSWORD;TrustServerCertificate=True;"
```

To set an API Key to secure the app, run this:

```sh
dotnet user-secrets set "X-API-KEY" "API_KEY_VALUE_HERE"
```

## Scaffolding

To scaffold models with the existing database, use this command line:

```sh
Scaffold-DbContext "Server=SERVERADDRESS;Database=DATABASE;User Id=USERID;Password=PASSWORD;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data/Entities -Force
```

# HTTPS

In production you need to run following commands to generate a certificate and use it in the app:

```sh
$ docker compose run --rm  certbot certonly --webroot --webroot-path /var/www/certbot/ -d example.org

```

_You will have to comment the server {...} section which refers to HTTPS 443 in the `.github/deploy/nginx/nginx.conf` file before running this command_

### Renewing certificates

```sh
$ docker compose run --rm  certbot certonly --webroot --webroot-path /var/www/certbot/ -d example.org
```
