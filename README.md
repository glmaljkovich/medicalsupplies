# Medical Supplies Backend

## Requirements

- .NET Core >= 3.1
- [Entity Framework CLI](#install-dotnet-ef)
- A Postgres database
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
dotnet build
```

## Run Migrations

```
DB_NAME="dbnetcore" DB_USERNAME="netcoreuser" DB_PASSWORD="netcorepass" dotnet ef database update
```

you can source a file to avoid writing password in terminal

## Run

```
DB_NAME="dbnetcore" DB_USERNAME="netcoreuser" DB_PASSWORD=netcorepass dotnet run
```

you can source a file to avoid writing password in terminal

The app will be available at http://localhost:5000

## API Documentation

The appication integrates swagger-ui application

You can read [api.yaml](https://github.com/glmaljkovich/medicalsupplies/blob/master/api.yaml) or browse:

```
http://<api-host>:<api-port>/swagger
```

## Logger

Use [Serilog](https://github.com/serilog/serilog-aspnetcore) to manage logg, first install 

```
dotnet add package Serilog.AspNetCore
```

To use Seq as a sink install:
```
dotnet add package Serilog.Sinks.Seq
```

And run Seq image:
```
docker run --rm -it -e ACCEPT_EULA=Y -p 5341:80 datalust/seq
```

## Create and run with docker


**Simple image**
```
docker build -t medicalsupplies .
```

```
docker run --rm -it \
 --name medicalsupplies \
 -e DB_URL=host.docker.internal \
 -e DB_NAME=dbnetcore \
 -e DB_USERNAME=netcoreuser \
 -e DB_PASSWORD=netcorepass \
 -p 8080:80 \
 medicalsupplies
```

**Riemann image**
```
docker build -t medicalsupplies-riemann -f DockerfileRiemann .
```

```
docker run --rm -it \
 --name medicalsupplies-riemann \
 -e RIEMANN_HOST=host.docker.internal \
 -e DB_URL=host.docker.internal \
 -e DB_NAME=dbnetcore \
 -e DB_USERNAME=netcoreuser \
 -e DB_PASSWORD=netcorepass \
 -p 8080:80 \
 medicalsupplies-riemann
```

## Load tests

**Requirements**
- artillery `npm i -g artillery`
- Create an admin user
  ```bash
  docker exec -it postgres psql -h localhost -U postgres
  ```

  ```sql
  \l
  \c dbnetcore
  \dt
  SELECT * FROM "Users";
  UPDATE "Users" SET "IsAdmin" = true WHERE "Email" = 'admin@admin.com';
  ```

- set these environment variables to the corresponding users in your DB
  ```bash
  export EMAIL=user@user.com
  export PASSWORD=user1234
  export ADMIN_EMAIL=admin@admin.com
  export ADMIN_PASSWORD=admin1234
  ```

**Run**

```bash
cd integration-tests
artillery run -o load-test-report.json load-test.yml
```

**Generate HTML report**
```bash
artillery report -o report.html load-test-report.json
```

**Reset databas**

```sql
DELETE FROM  public."__EFMigrationsHistory";
DROP TABLE public."Users";
DROP TABLE public."SupplyAttributes";
DROP TABLE public."Supplies";
DROP TABLE public."SuppliesOrders";
DROP TABLE public."Accounts";
DROP TABLE public."SupplyTypeAttributes";
DROP TABLE public."OrganizationSupplyTypes";
DROP TABLE public."SupplyTypes";
DROP TABLE public."Organizations";
DROP TABLE public."Areas";
```

#### build & update docker-compose-container

```bash
docker-compose up -d --no-deps --build <service_name>
#eg: docker-compose up -d --no-deps --build app
```

#### build & update docker-compose-container

```bash
docker-compose up -d --no-deps <service_name>
#eg: docker-compose up -d --no-deps app
```