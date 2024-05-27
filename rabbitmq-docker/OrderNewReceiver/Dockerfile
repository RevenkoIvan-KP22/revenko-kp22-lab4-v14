FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrderNewReceiver.csproj", "OrderNewReceiver/"]
RUN dotnet restore "./OrderNewReceiver/OrderNewReceiver.csproj"
WORKDIR "/src/OrderNewReceiver"
COPY . .

RUN dotnet build "./OrderNewReceiver.csproj" -c Release -o /app/build
RUN dotnet publish "./OrderNewReceiver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderNewReceiver.dll"]