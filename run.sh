docker-compose down
docker-compose up -d
docker exec -d  medicalsupplies-riemann dotnet ef database update