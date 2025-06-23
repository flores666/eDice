FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 85

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY AssetCrafterService/ AssetCrafterService/
COPY Infrastructure.AssetCrafterService/ Infrastructure.AssetCrafterService/
COPY Shared.Lib/ Shared.Lib/
COPY Shared.Logging/ Shared.Logging/
COPY Shared.Models/ Shared.Models/
COPY Shared.MessageBus.Kafka/ Shared.MessageBus.Kafka/

RUN dotnet restore "AssetCrafterService/AssetCrafterService.csproj"
RUN dotnet build   "AssetCrafterService/AssetCrafterService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "AssetCrafterService/AssetCrafterService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssetCrafterService.dll"]
