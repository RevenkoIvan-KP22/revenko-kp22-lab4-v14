FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrderProcessingReceiver.csproj", "OrderProcessingReceiver/"]
COPY ["OrderNewReceiver.csproj", "OrderNewReceiver/"]
RUN dotnet restore "./OrderProcessingReceiver/OrderProcessingReceiver.csproj"
WORKDIR "/src/OrderProcessingReceiver"
COPY . .

RUN dotnet build "./OrderProcessingReceiver.csproj" -c Release -o /app/build
RUN dotnet publish "./OrderProcessingReceiver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderProcessingReceiver.dll"]