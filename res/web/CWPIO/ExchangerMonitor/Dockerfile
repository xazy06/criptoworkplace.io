FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/ExchangerMonitor"
RUN dotnet publish "ExchangerMonitor.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ExchangerMonitor.dll"]