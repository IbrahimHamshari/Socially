# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

# Copy the Directory.Packages.props file for centralized package management
COPY ["Socially.UserManagment/Directory.Packages.props", "Socially.UserManagment/"]
COPY ["Socially.UserManagment/Directory.Build.props", "Socially.UserManagment/"]


# Copy the .csproj files for all projects
COPY ["Socially.UserManagment/src/Socially.UserManagment.Web/Socially.UserManagment.Web.csproj", "Socially.UserManagment/src/Socially.UserManagment.Web/"]
COPY ["Socially.UserManagment/src/Socially.UserManagment.Shared/Socially.UserManagment.Shared.csproj", "Socially.UserManagment/src/Socially.UserManagment.Shared/"]
COPY ["Socially.UserManagment/src/Socially.UserManagment.Infrastructure/Socially.UserManagment.Infrastructure.csproj", "Socially.UserManagment/src/Socially.UserManagment.Infrastructure/"]
COPY ["Socially.UserManagment/src/Socially.UserManagment.Core/Socially.UserManagment.Core.csproj", "Socially.UserManagment/src/Socially.UserManagment.Core/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["Socially.UserManagment/src/Socially.UserManagment.UseCases/Socially.UserManagment.UseCases.csproj", "Socially.UserManagment/src/Socially.UserManagment.UseCases/"]

# Perform the restore with the centralized package versioning enabled
RUN dotnet restore "./Socially.UserManagment/src/Socially.UserManagment.Web/Socially.UserManagment.Web.csproj"

COPY . .
WORKDIR "/src/Socially.UserManagment/src/Socially.UserManagment.Web"
RUN dotnet build "./Socially.UserManagment.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Socially.UserManagment.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Socially.UserManagment.Web.dll"]
