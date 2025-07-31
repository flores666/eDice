FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# в контексте уже лежат все файлы проекта (csproj, Program.cs, Controllers и т.д.)
COPY AuthorizationService/ AuthorizationService/
COPY Infrastructure.AuthorizationService/ Infrastructure.AuthorizationService/
COPY Shared.Lib/ Shared.Lib/
COPY Shared.Logging/ Shared.Logging/
COPY Shared.Models/ Shared.Models/
COPY Shared.MessageBus.Kafka/ Shared.MessageBus.Kafka/

RUN dotnet restore "AuthorizationService/AuthorizationService.csproj"
RUN dotnet build   "AuthorizationService/AuthorizationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "AuthorizationService/AuthorizationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizationService.dll"]
