FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 83

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# в контексте уже лежат все файлы проекта (csproj, Program.cs, Controllers и т.д.)
COPY MailService/ MailService/
COPY Shared.Logging/ Shared.Logging/
COPY Shared.Models/ Shared.Models/
COPY Shared.MessageBus.Kafka/ Shared.MessageBus.Kafka/

RUN dotnet restore "MailService/MailService.csproj"
RUN dotnet build   "MailService/MailService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "MailService/MailService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Установить libkrb5
RUN apt-get update && apt-get install -y libkrb5-3 && rm -rf /var/lib/apt/lists/*

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MailService.dll"]
