FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 86

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY UsersCoordinatorService/ UsersCoordinatorService/
COPY Shared.Lib/ Shared.Lib/
COPY Shared.Logging/ Shared.Logging/
COPY Shared.MessageBus.Kafka/ Shared.MessageBus.Kafka/

RUN dotnet restore "UsersCoordinatorService/UsersCoordinatorService.csproj"
RUN dotnet build   "UsersCoordinatorService/UsersCoordinatorService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "UsersCoordinatorService/UsersCoordinatorService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsersCoordinatorService.dll"]
