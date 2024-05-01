FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AzureB2C.Data.Migrator/AzureB2C.Data.Migrator.csproj", "AzureB2C.Data.Migrator/"]
RUN dotnet restore "AzureB2C.Data.Migrator/AzureB2C.Data.Migrator.csproj"
COPY . .
WORKDIR "/src/AzureB2C.Data.Migrator"
RUN dotnet build "AzureB2C.Data.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AzureB2C.Data.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureB2C.Data.Migrator.dll"]
