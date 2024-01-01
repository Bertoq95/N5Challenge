FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app

COPY ./N5Challenge/bin/Debug/net6.0/ .

COPY ./N5Challenge.Application/bin/Debug/net6.0/ ./N5Challenge.Application
COPY ./N5Challenge.Domain/bin/Debug/net6.0/      ./N5Challenge.Domain
COPY ./N5Challenge.Infrastructure/bin/Debug/net6.0/  ./N5Challenge.Infrastructure

ENTRYPOINT ["dotnet", "N5Challenge.dll"]