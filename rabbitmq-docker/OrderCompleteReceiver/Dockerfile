FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrderCompleteReceiver.csproj", "OrderCompleteReceiver/"]
COPY ["OrderNewReceiver.csproj", "OrderNewReceiver/"]
RUN dotnet restore "./OrderCompleteReceiver/OrderCompleteReceiver.csproj"
WORKDIR "/src/OrderCompleteReceiver"
COPY . .

RUN dotnet build "./OrderCompleteReceiver.csproj" -c Release -o /app/build
RUN dotnet publish "./OrderCompleteReceiver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderCompleteReceiver.dll"]