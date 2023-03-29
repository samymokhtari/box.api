# Box

## Description

Here is a project which is an API to store some file on a server. It's a simple API developed with ASP.NET Core.

You can find below the architecture designed which represent all resources used and interactions in this project
[![Architecture](http://image.noelshack.com/fichiers/2022/48/3/1669835004-box.png)]

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
`dotnet user-secrets set "ConnectionStrings:ConnStr" "Server=SERVERADDRESS;Database=DATABASE;User ID=USERID;Password=PASSWORD;TrustServerCertificate=True;"`

## Scaffolding

To scaffold models with the existing database, use this command line:
```
Scaffold-DbContext "Server=SERVERADDRESS;Database=DATABASE;User Id=USERID;Password=PASSWORD;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data/Entities -Force
```