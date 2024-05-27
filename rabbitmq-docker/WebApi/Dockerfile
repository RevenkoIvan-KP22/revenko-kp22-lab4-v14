#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApi.csproj", "WebApi/"]
RUN dotnet restore "./WebApi/WebApi.csproj"
WORKDIR "/src/WebApi"
COPY . .
RUN dotnet build "./WebApi.csproj" -c Release -o /app/build
RUN dotnet publish "./WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]