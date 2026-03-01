FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GtMotive.Estimate.Microservice.PU/GtMotive.Estimate.Microservice.PU.csproj", "GtMotive.Estimate.Microservice.PU/"]
RUN dotnet restore "GtMotive.Estimate.Microservice.PU/GtMotive.Estimate.Microservice.PU.csproj"
COPY . .
WORKDIR "/src/GtMotive.Estimate.Microservice.PU"
RUN dotnet build "./GtMotive.Estimate.Microservice.PU.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GtMotive.Estimate.Microservice.PU.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GtMotive.Estimate.Microservice.PU.dll"]
