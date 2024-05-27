FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrderCancelledReceiver.csproj", "OrderCancelledReceiver/"]
COPY ["OrderNewReceiver.csproj", "OrderNewReceiver/"]
RUN dotnet restore "./OrderCancelledReceiver/OrderCancelledReceiver.csproj"
WORKDIR "/src/OrderCancelledReceiver"
COPY . .

RUN dotnet build "./OrderCancelledReceiver.csproj" -c Release -o /app/build
RUN dotnet publish "./OrderCancelledReceiver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderCancelledReceiver.dll"]