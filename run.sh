#!/bin/sh

export COMPOSE_PROJECT_NAME=medicalsupplies

docker-compose down
docker-compose up -d

docker run --rm -it \
  -v ${PWD}/.config:/workspace/.config \
  -v ${PWD}/api.yaml:/workspace/api.yaml \
  -v ${PWD}/AppSettings.cs:/workspace/AppSettings.cs \
  -v ${PWD}/appsettings.json:/workspace/appsettings.json \
  -v ${PWD}/ArqNetCore.csproj:/workspace/ArqNetCore.csproj \
  -v ${PWD}/Program.cs:/workspace/Program.cs \
  -v ${PWD}/Startup.cs:/workspace/Startup.cs \
  -v ${PWD}/Configuration:/workspace/Configuration \
  -v ${PWD}/Controllers:/workspace/Controllers \
  -v ${PWD}/DTOs:/workspace/DTOs \
  -v ${PWD}/Entities:/workspace/Entities \
  -v ${PWD}/Exceptions:/workspace/Exceptions \
  -v ${PWD}/Mappers:/workspace/Mappers \
  -v ${PWD}/Migrations:/workspace/Migrations \
  -v ${PWD}/Services:/workspace/Services \
  -v ${PWD}/Services:/workspace/Services \
  -w /workspace \
  -e DB_NAME=dbnetcore \
  -e DB_USERNAME=netcoreuser \
  -e DB_PASSWORD=netcorepass \
  -e DB_URL=192.168.0.31 \
  mcr.microsoft.com/dotnet/core/sdk:3.1 \
  /bin/bash -c "dotnet restore; dotnet tool restore; dotnet ef database update"
