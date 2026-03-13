FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ECommerce.Worker/ECommerce.Worker.csproj", "ECommerce.Worker/"]
RUN dotnet restore "ECommerce.Worker/ECommerce.Worker.csproj"

COPY . .
WORKDIR "/src/ECommerce.Worker"
RUN dotnet build "ECommerce.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerce.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ECommerce.Worker.dll"]