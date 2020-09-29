# Medical Supplies Backend

## Requirements

- .NET Core >= 3.1
- [Entity Framework CLI](#install-dotnet-ef)
- A MySQL database
- Docker >= 19.0 (only development)



## Setup

If you don't have one already, you can create a development database with this docker command

```bash
docker run -d \
  --name postgres \
  -e POSTGRES_PASSWORD=123456 \
  -p 5432:5432 \
  postgres
```

### Install dotnet-ef

run
```
dotnet tool install --global dotnet-ef
```

Add to `.bashrc` or `.zshrc`
```
export PATH="$PATH:$HOME/.dotnet/tools/"
```

#### **PostgreSQL**
- Create a database in **PostgreSQL** server and provide access to user

  ```bash
  docker exec -it postgres psql -h localhost -U postgres
  ```

  ```sql
  create database dbnetcore;
  create user netcoreuser with encrypted password 'netcorepass';
  grant all privileges on database dbnetcore to netcoreuser;
  ```

## Build

```
cd api
dotnet build
```

## Run Migrations

```
DB_NAME="dbnetcore" DB_USERNAME="netcoreuser" DB_PASSWORD="netcorepass" dotnet ef database update
```

you can source a file to avois writing password in terminal

## Run

```
cd api
DB_NAME="dbnetcore" DB_USERNAME="netcoreuser" DB_PASSWORD=netcorepass dotnet run
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


Simple image
```
docker build -t medicalsupplies .
```

Riemann image
```
docker build -t medicalsupplies-riemann -f DockerfileRiemann .
```

```
docker run --rm -it \
 --name medicalsupplies \
 -e DB_URL=192.168.0.31 \
 -e DB_NAME=dbnetcore \
 -e DB_USERNAME=netcoreuser \
 -e DB_PASSWORD=netcorepass \
 -p 8080:80 \
 medicalsupplies
```

```
docker run --rm -it \
 --name medicalsupplies-riemann \
 -e DB_URL=192.168.0.31 \
 -e DB_NAME=dbnetcore \
 -e DB_USERNAME=netcoreuser \
 -e DB_PASSWORD=netcorepass \
 -p 8080:80 \
 medicalsupplies-riemann
```