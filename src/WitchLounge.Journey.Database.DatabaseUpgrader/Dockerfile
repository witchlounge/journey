# build application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Database Updater references
# TODO single COPY command ?
COPY ./WitchLounge.Journey.Database.DatabaseUpgrader ./WitchLounge.Journey.Database.DatabaseUpgrader
COPY ./WitchLounge.Journey.Database ./WitchLounge.Journey.Database
COPY ./WitchLounge.Journey.Common ./WitchLounge.Journey.Common

WORKDIR /src/WitchLounge.Journey.Database.DatabaseUpgrader

RUN dotnet restore WitchLounge.Journey.Database.DatabaseUpgrader.csproj
RUN dotnet publish -c Release -o ../../app WitchLounge.Journey.Database.DatabaseUpgrader.csproj

# run application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app .
COPY --from=build [ "/src/WitchLounge.Journey.Database.DatabaseUpgrader/appsettings.json", "/src/WitchLounge.Journey.Database.DatabaseUpgrader/appsettings.Production.json", "./" ]

ENTRYPOINT [ "dotnet", "WitchLounge.Journey.Database.DatabaseUpgrader.dll" ]