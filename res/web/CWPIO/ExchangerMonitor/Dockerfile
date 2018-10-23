FROM microsoft/aspnetcore:2.1 AS base
WORKDIR /app

FROM microsoft/aspnetcore-build:2.1 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/ExchangerMonitor"
RUN dotnet build "ExchangerMonitor.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ExchangerMonitor.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ExchangerMonitor.dll"]