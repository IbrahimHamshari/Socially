#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Copy the Directory.Packages.props file for centralized package management
COPY ["Socially.Messaging/Directory.Packages.props", "Socially.Messaging/"]
COPY ["Socially.Messaging/Directory.Build.props", "Socially.Messaging/"]


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Socially.Messaging/src/Socially.Messaging.Web/Socially.Messaging.Web.csproj", "Socially.Messaging/src/Socially.Messaging.Web/"]
COPY ["Socially.Messaging/src/Socially.Messaging.Infrastructure/Socially.Messaging.Infrastructure.csproj", "Socially.Messaging/src/Socially.Messaging.Infrastructure/"]
COPY ["Socially.Messaging/src/Socially.Messaging.Core/Socially.Messaging.Core.csproj", "Socially.Messaging/src/Socially.Messaging.Core/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["Socially.Messaging/src/Socially.Messaging.UseCases/Socially.Messaging.UseCases.csproj", "Socially.Messaging/src/Socially.Messaging.UseCases/"]
RUN dotnet restore "./Socially.Messaging/src/Socially.Messaging.Web/Socially.Messaging.Web.csproj"
COPY . .
WORKDIR "/src/Socially.Messaging/src/Socially.Messaging.Web"
RUN dotnet build "./Socially.Messaging.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Socially.Messaging.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Socially.Messaging.Web.dll"]