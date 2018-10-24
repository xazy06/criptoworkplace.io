FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
WORKDIR "/src/ExchangerMonitor"
RUN dotnet build "ExchangerMonitor.csproj" -c Release -o /app
RUN dotnet publish "ExchangerMonitor.csproj" -c Release -o /pub

FROM microsoft/dotnet:2.1-runtime AS final
WORKDIR /app
COPY --from=build /pub .
RUN ls
ENTRYPOINT ["dotnet", "ExchangerMonitor.dll"]