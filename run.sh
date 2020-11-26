#!/bin/sh

export COMPOSE_PROJECT_NAME=medicalsupplies

docker-compose down
docker-compose up -d
docker exec -d  medicalsupplies-riemann dotnet ef database update