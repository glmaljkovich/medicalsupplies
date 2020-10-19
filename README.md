# Medical Supplies Backend

## Requirements

- .NET Core >= 3.1
- [Entity Framework CLI](#install-dotnet-ef)
- A MySQL database
- Docker >= 19.0 (only development)



## Setup

If you don't have one already, you can create a development database with this docker command

docker run --name postgres -e POSTGRES_USER=netcoreuser -e POSTGRES_PASSWORD=netcorepass -p 5432:5432 -d postgres

### Install dotnet-ef

run
```
dotnet tool install --global dotnet-ef
```

Add to `.bashrc` or `.zshrc`
```
export PATH="$PATH:$HOME/.dotnet/tools/"
```


#### **MySQL**
- Create a database in **MySQL** server and provide access to user

  ``` sql
  CREATE DATABASE dbnetcore;
  CREATE USER 'netcoreuser'@'%' IDENTIFIED BY 'netcorepass';
  GRANT ALL PRIVILEGES ON dbnetcore. * TO 'netcoreuser'@'%';
  FLUSH PRIVILEGES;
  ```
- Create initial Schema

  ```sql
  CREATE TABLE `__EFMigrationsHistory` 
  ( 
    `MigrationId` nvarchar(150) NOT NULL, 
    `ProductVersion` nvarchar(32) NOT NULL, 
     PRIMARY KEY (`MigrationId`) 
  );
  ```

## Build

```
cd api
dotnet build
```

## Run Migrations

```
DB_NAME="medicalsupplies" DB_USERNAME="netcoreuser" DB_PASSWORD=netcorepass dotnet ef database update
```

you can source a file to avois writing password in terminal

## Run

```
cd api
DB_NAME="medicalsupplies" DB_USERNAME="netcoreuser" DB_PASSWORD=netcorepass dotnet run
```

you can source a file to avois writing password in terminal

The app will be available at http://localhost:5000

## API Documentation

The appication integrates swagger-ui application

You can read [api.yaml]() or browse:

```
http://<api-host>:<api-port>/swagger
```

## Logger

Use [Serilog](https://github.com/serilog/serilog-aspnetcore) to manage logg, first install 

```
dotnet add package Serilog.AspNetCore
```

For use Seq install:
```
dotnet add package Serilog.Sinks.Seq
```

And run Seq image:
```
docker run --rm -it -e ACCEPT_EULA=Y -p 5341:80 datalust/seq
```

## create and run with docker

```
docker build \
 -e DB_URL=192.168.0.31 \
 -e DB_NAME=dbnetcore \
 -e DB_USERNAME=netcoreuser \
 -e DB_PASSWORD=netcorepass \
 -t medicalsupplies .
```

```
docker run --rm -it \
 -e DB_URL=192.168.0.31 \
 -e DB_NAME=dbnetcore \
 -e DB_USERNAME=netcoreuser \
 -e DB_PASSWORD=netcorepass \
 -p 8080:80 \
 --network medisuplinewouwu \
 --env-file .env \
 medicalsupplies
```

## Run Datadog agent
```
docker network create medisuplinewouwu
docker run -d --name datadog-agent \
          --network medisuplinewouwu \
          -v /var/run/docker.sock:/var/run/docker.sock:ro \
          -v /proc/:/host/proc/:ro \
          -v /sys/fs/cgroup/:/host/sys/fs/cgroup:ro \
          -e DD_API_KEY=1d720a286c4f841d8ebef67dbf5d70ac \
          -e DD_APM_ENABLED=true \
          -e DD_APM_NON_LOCAL_TRAFFIC=true \
          datadog/agent:latest

```