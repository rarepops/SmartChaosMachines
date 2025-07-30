# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image for runtime
FROM mcr.microsoft.com/dotnet/runtime:10.0-preview AS base
WORKDIR /app

# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and restore
COPY ["SmartChaosMachines.LineControl/SmartChaosMachines.LineControl.csproj", "SmartChaosMachines.LineControl/"]
RUN dotnet restore "./SmartChaosMachines.LineControl/SmartChaosMachines.LineControl.csproj"

COPY . .
WORKDIR "/src/SmartChaosMachines.LineControl"
RUN dotnet build "./SmartChaosMachines.LineControl.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SmartChaosMachines.LineControl.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartChaosMachines.LineControl.dll"]
