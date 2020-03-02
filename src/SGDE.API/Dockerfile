FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY Gestamp.sln .
COPY docker-compose.dcproj /src/
COPY src/SGDE.API/SGDE.API.csproj src/SGDE.API/
COPY src/SGDE.Domain/SGDE.Domain.csproj src/SGDE.Domain/
COPY src/SGDE.DataEFCoreMongoDB/SGDE.DataEFCoreMongoDB.csproj src/SGDE.DataEFCoreMongoDB/
COPY src/SGDE.DataEFCoreSQL/SGDE.DataEFCoreSQL.csproj src/SGDE.DataEFCoreSQL/
COPY src/SGDE.SeedData/SGDE.SeedData.csproj src/SGDE.SeedData/
COPY src/SGDE.Web/SGDE.Web.csproj src/SGDE.Web/

RUN dotnet restore Gestamp.sln

COPY . .
WORKDIR /src/src/SGDE.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SGDE.API.dll"]