# -------- BUILD STAGE --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# csproj dosyalarýný kopyala
COPY ECommerce.API/ECommerce.API.csproj ECommerce.API/
COPY ECommerce.Application/ECommerce.Application.csproj ECommerce.Application/
COPY ECommerce.Persistence/ECommerce.Persistence.csproj ECommerce.Persistence/

# restore
RUN dotnet restore ECommerce.API/ECommerce.API.csproj

# tüm kodu kopyala
COPY . .

WORKDIR /src/ECommerce.API

# publish
RUN dotnet publish -c Release -o /app/publish

# -------- RUNTIME STAGE --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerce.API.dll"]