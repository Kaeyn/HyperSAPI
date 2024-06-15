#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Comment : Ctrl+K + Ctrl+C

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["HyperSAPI.csproj", "."]
#RUN dotnet restore "./HyperSAPI.csproj"
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "./HyperSAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./HyperSAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "HyperSAPI.dll"]

# Use the official .NET 8.0 SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY APP.API/APP.API.csproj HyperSAPI/
#COPY APP.Bus.csproj ../APP.Bus
#COPY APP.DAL.csproj ../APP.DAL

RUN dotnet restore HyperSAPI/APP.API.csproj

COPY . .
WORKDIR /src/APP.API
RUN dotnet publish APP.API.csproj -c Release -o /app/build
# Copy everything else and build

FROM build AS publish
RUN dotnet publish APP.API.csproj -c Release -o /app/publish

## Build runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:8.0
#WORKDIR /app
#COPY --from=out /app/out .
#ENTRYPOINT ["dotnet", "APP.Api.dll"]

# Stage 2: Create the runtime image
FROM base AS final
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "HyperSAPI.dll"]