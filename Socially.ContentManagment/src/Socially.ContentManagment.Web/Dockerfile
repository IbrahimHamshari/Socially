#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the Directory.Packages.props file for centralized package management
COPY ["Socially.UserManagment/Directory.Packages.props", "Socially.UserManagment/"]
COPY ["Socially.UserManagment/Directory.Build.props", "Socially.UserManagment/"]

COPY ["Socially.ContentManagment/src/Socially.ContentManagment.Web/Socially.ContentManagment.Web.csproj", "Socially.ContentManagment/src/Socially.ContentManagment.Web/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["Socially.ContentManagment/src/Socially.ContentManagment.Infrastructure/Socially.ContentManagment.Infrastructure.csproj", "Socially.ContentManagment/src/Socially.ContentManagment.Infrastructure/"]
COPY ["Socially.ContentManagment/src/Socially.ContentManagment.Core/Socially.ContentManagment.Core.csproj", "Socially.ContentManagment/src/Socially.ContentManagment.Core/"]
COPY ["Socially.ContentManagment/src/Socially.ContentManagment.UseCases/Socially.ContentManagment.UseCases.csproj", "Socially.ContentManagment/src/Socially.ContentManagment.UseCases/"]
RUN dotnet restore "./Socially.ContentManagment/src/Socially.ContentManagment.Web/Socially.ContentManagment.Web.csproj"
COPY . .
WORKDIR "/src/Socially.ContentManagment/src/Socially.ContentManagment.Web"
RUN dotnet build "./Socially.ContentManagment.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Socially.ContentManagment.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Socially.ContentManagment.Web.dll"]