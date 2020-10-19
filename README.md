# Medical Supplies Backend

## Requirements

- .NET Core >= 3.1
- [Entity Framework CLI](#install-dotnet-ef)
- A MySQL database
- Docker >= 19.0 (only development)



## Setup

If you don't have one already, you can create a development database with this docker command
```
docker run --name dbnetcore \
  -e POSTGRES_USER=netcoreuser \
  -e POSTGRES_PASSWORD=netcorepass \
  --network medisuplinewouwu \
  -p 5432:5432 \
  -d postgres
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

## Build

```
dotnet build
```

## Run Migrations

```
DB_NAME="dbnetcore" DB_USERNAME="netcoreuser" DB_PASSWORD=netcorepass dotnet ef database update
```

you can source a file to avoid writing password in terminal

## Run

```
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
create the network
```
docker network create medisuplinewouwu
```
run the app
```
docker run --rm -it \
 -e DB_URL=dbnetcore \
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