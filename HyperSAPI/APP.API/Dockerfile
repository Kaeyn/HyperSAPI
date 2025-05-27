##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
##Comment : Ctrl+K + Ctrl+C
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
## Use the official .NET 8.0 SDK image
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#
#COPY APP.API/APP.API.csproj HyperSAPI/
#
#RUN dotnet restore HyperSAPI/APP.API.csproj
#
#COPY . .
#WORKDIR /src/APP.API
#RUN dotnet publish APP.API.csproj -c Release -o /app/build
## Copy everything else and build
#
#FROM build AS publish
#RUN dotnet publish APP.API.csproj -c Release -o /app/publish
#
## Stage 2: Create the runtime image
#FROM base AS final
#WORKDIR /app
#
## Copy the built application from the build stage
#COPY --from=publish /app/publish .
#
## Set the entry point for the application
#ENTRYPOINT ["dotnet", "APP.API.dll"]
# Build stage

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy sln and restore
COPY HyperSAPI.sln .
COPY APP.API.csproj APP.API/
COPY ../APP.Bus/APP.Bus.csproj APP.Bus/
COPY ../APP.DAL/APP.DAL.csproj APP.DAL/

RUN dotnet restore APP.API/APP.API.csproj

# Copy everything else and publish
COPY . .
WORKDIR /src/APP.API
RUN dotnet publish App.API.csproj -c Release -o /app/publish

# ------------ Runtime Stage ------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "APP.API.dll"]