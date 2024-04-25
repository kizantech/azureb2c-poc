FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AzureAdB2BApi/AzureAdB2BApi.csproj", "AzureAdB2BApi/"]
RUN dotnet restore "AzureAdB2BApi/AzureAdB2BApi.csproj"
COPY . .
WORKDIR "/src/AzureAdB2BApi"
RUN dotnet build "AzureAdB2BApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AzureAdB2BApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureAdB2BApi.dll"]
