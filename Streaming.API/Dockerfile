#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Streaming.API/Streaming.API.csproj", "Streaming.API/"]
COPY ["Streaming.Application/Streaming.Application.csproj", "Streaming.Application/"]
COPY ["Streaming.Repository/Streaming.Repository.csproj", "Streaming.Repository/"]
COPY ["Streaming.Domain/Streaming.Domain.csproj", "Streaming.Domain/"]
COPY . .
WORKDIR "/src/Streaming.API"
RUN dotnet publish "./Streaming.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Streaming.API.dll"]