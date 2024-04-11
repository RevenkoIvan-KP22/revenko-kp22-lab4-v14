#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ContinentsReceiver/ContinentsReceiver.csproj", "ContinentsReceiver/"]
RUN dotnet restore "ContinentsReceiver/ContinentsReceiver.csproj"
COPY . .
WORKDIR "/src/ContinentsReceiver"
RUN dotnet build "ContinentsReceiver.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContinentsReceiver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContinentsReceiver.dll"]