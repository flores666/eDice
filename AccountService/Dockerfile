FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# в контексте уже лежат все файлы проекта (csproj, Program.cs, Controllers и т.д.)
COPY AccountService/ AccountService/
COPY Infrastructure.AccountService/ Infrastructure.AccountService/
COPY Shared.Lib/ Shared.Lib/
COPY Shared.Logging/ Shared.Logging/
COPY Shared.Models/ Shared.Models/
COPY Shared.MessageBus.Kafka/ Shared.MessageBus.Kafka/

RUN dotnet restore "AccountService/AccountService.csproj"
RUN dotnet build   "AccountService/AccountService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "AccountService/AccountService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountService.dll"]
