#### Usage
The appication integrates swagger-ui application
http://<api-host>:<api-port>/swagger


#### Installation
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
- Set environment variables in terminal and run migrations
  ```bash
  $ export DB_NAME=dbnetcore;export DB_USERNAME=netcoreuser; export DB_PASSWORD=netcorepass
  $ dotnet ef database update
  ```
  Install **Entity Framework** could be required
  ```bash
  $ dotnet tool install --global dotnet-ef
  ```
  Set dotnet tool path could be required
  ```bash
  $ export PATH="$PATH:/home/principal/.dotnet/tools"
  ```
- Install dependencies

  ```bash
  $ dotnet restore
  ```
- Execute app (this required environment variables)
  ```bash
  $ dotnet run
  ```
