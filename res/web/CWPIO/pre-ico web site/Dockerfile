FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/pre-ico web site"
RUN dotnet build "pre-ico web site.csproj" -c Release -o /app
RUN dotnet publish "pre-ico web site.csproj" -c Release -o /pub

FROM microsoft/aspnetcore:2.0 AS final
WORKDIR /app
COPY --from=build /pub .
EXPOSE 80
ENTRYPOINT ["dotnet", "pre-ico web site.dll"]