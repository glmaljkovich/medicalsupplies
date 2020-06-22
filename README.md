# Medical Supplies Backend

## Requirements

- .NET Core >= 3.1
- [Entity Framework CLI](#install-dotnet-ef)
- Docker >= 19.0
- A MySQL database

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

## Build

```
cd api
dotnet build
```

## Run Migrations

```
DB_NAME="medicalsupplies" DB_USERNAME="root" dotnet ef database update
```

## Run

```
cd api
DB_NAME="medicalsupplies" DB_USERNAME="root" dotnet run
```

The app will be available at http://localhost:5000

## API Documentation

You can read [api.yaml]() or browse:

```
http://localhost:5000/swagger
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