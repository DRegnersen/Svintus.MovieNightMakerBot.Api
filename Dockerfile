﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Svintus.MovieNightMakerBot.Api/Svintus.MovieNightMakerBot.Api.csproj", "Svintus.MovieNightMakerBot.Api/"]
COPY ["Svintus.MovieNightMakerBot.Core/Svintus.MovieNightMakerBot.Core.csproj", "Svintus.MovieNightMakerBot.Core/"]
RUN dotnet restore "Svintus.MovieNightMakerBot.Api/Svintus.MovieNightMakerBot.Api.csproj"

COPY . .
WORKDIR "/src/Svintus.MovieNightMakerBot.Api"
RUN dotnet build "Svintus.MovieNightMakerBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Svintus.MovieNightMakerBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Svintus.MovieNightMakerBot.Api.dll"]