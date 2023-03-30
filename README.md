# WCOW Docker compose accessing host endpoint example

This repository contains an example of using Windows Containers on Windows (WCOW)
with a [`docker-compose.yml`](./docker-compose.yml). The focus on this example
is to get a container to be able to make a call to a port on the Docker host.
Out of the box WCOW Docker compose can't reach endpoints on the Docker host,
two things are necessary to make that work:

1. A custom network needs to be created for the Docker compose.
1. A Firewall rule needs to be created to allow the containers to access
  the host endpoint.

Pre-requisites to test this example:

1. Switch the Docker daemon to "Windows Containers".
1. Have administrator privileges to create the Firewall rule.

Testing the example, open two PowerShell prompts. Use the first to build,
start the Docker compose, and run the host endpoint:

```PowerShell
> .\Build.ps1
New-NetFirewallRule -DisplayName 'wocw-host-port' -Direction Inbound -LocalAddress 10.1.1.1 -LocalPort 5000 -Protocol TCP -Action Allow -Profile Any
docker compose up -d
$Env:ASPNETCORE_URLS="http://10.1.1.1:5000" # the use of 10.1.1.1 here requires the docker compose network to be already in place
.\App\ServerApp.exe
```

After the host endpoint is up and running use the second PowerShell prompt
to issue a request to one of the Docker containers:

```PowerShell
curl http://localhost:5001/forward
```

This request will result in a request between the Docker containers, and
from the 2nd Docker container to the endpoint on the host.

After the test is complete, stop the containers, and remove the Firewall rule:

```PowerShell
docker compose down
Remove-NetFirewallRule -DisplayName 'wocw-host-port'
```
