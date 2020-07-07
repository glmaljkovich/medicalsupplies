# Medical Supplies Backend

## Requirements

- .NET Core >= 3.1
- [Entity Framework CLI](#install-dotnet-ef)
- A MySQL database
- Docker >= 19.0 (only development)



## Setup

If you don't have one already, you can create a development database with this docker command

docker run --name mysql -e MYSQL_ALLOW_EMPTY_PASSWORD=true -p 3306:3306 -d mysql

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
DB_NAME="medicalsupplies" DB_USERNAME="root" DB_PASSWORD=netcorepass dotnet ef database update
```

you can source a file to avois writing password in terminal

## Run

```
cd api
DB_NAME="medicalsupplies" DB_USERNAME="root" DB_PASSWORD=netcorepass dotnet run
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
docker build -t medicalsupplies .
```

```
docker run --rm -it \
 -e DB_URL=192.168.0.31 \
 -e DB_NAME=dbnetcore \
 -e DB_USERNAME=netcoreuser \
 -e DB_PASSWORD=netcorepass \
 -p 8080:80 \
 medicalsupplies
```