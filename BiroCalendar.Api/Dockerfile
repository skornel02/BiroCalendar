#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

LABEL maintainer="Stefán Kornél <contact@skornel02.hu>"
LABEL repository="https://github.com/skornel02/BiroCalendar"
LABEL homepage="https://github.com/skornel02/BiroCalendar"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BiroCalendar.Api/BiroCalendar.Api.csproj", "BiroCalendar.Api/"]
RUN dotnet restore "./BiroCalendar.Api/./BiroCalendar.Api.csproj"
COPY . .
WORKDIR "/src/BiroCalendar.Api"
RUN dotnet build "./BiroCalendar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BiroCalendar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BiroCalendar.Api.dll"]