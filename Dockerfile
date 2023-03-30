#escape=`

ARG windowscontainer_version=ltsc2019
FROM mcr.microsoft.com/windows/servercore:${windowscontainer_version}

COPY ./App/ /App

ENV ASPNETCORE_URLS=http://*:5000

ENTRYPOINT "C:\App\ServerApp.exe"
