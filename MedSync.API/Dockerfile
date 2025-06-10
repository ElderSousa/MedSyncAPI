# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MedSync.API/MedSync.API.csproj", "MedSync.API/"]
COPY ["MedSync.CrossCutting/MedSync.CrossCutting.csproj", "MedSync.CrossCutting/"]
COPY ["MedSync.Domain/MedSync.Domain.csproj", "MedSync.Domain/"]
COPY ["MedSync.Application/MedSync.Application.csproj", "MedSync.Application/"]
COPY ["MedSync.Infrastructure/MedSync.Infrastructure.csproj", "MedSync.Infrastructure/"]
RUN dotnet restore "./MedSync.API/MedSync.API.csproj"
COPY . .
WORKDIR "/src/MedSync.API"
RUN dotnet build "./MedSync.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MedSync.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedSync.API.dll"]