#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AfricaReceiver.csproj", "AfricaReceiver/"]
RUN dotnet restore "./AfricaReceiver/AfricaReceiver.csproj"
WORKDIR "/src/AfricaReceiver"
COPY . .

RUN dotnet build "./AfricaReceiver.csproj" -c Release -o /app/build
RUN dotnet publish "./AfricaReceiver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AfricaReceiver.dll"]