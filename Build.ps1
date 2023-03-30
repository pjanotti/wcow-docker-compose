dotnet publish .\ServerApp --use-current-runtime --self-contained -c Release -o .\App
docker build . -t forward-server
docker compose build
