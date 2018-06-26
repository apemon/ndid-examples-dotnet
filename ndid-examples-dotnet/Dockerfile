FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ndid-examples-dotnet/ndid-examples-dotnet.csproj ndid-examples-dotnet/
RUN dotnet restore ndid-examples-dotnet/ndid-examples-dotnet.csproj
COPY . .
WORKDIR /src/ndid-examples-dotnet
RUN dotnet build ndid-examples-dotnet.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ndid-examples-dotnet.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ndid-examples-dotnet.dll"]
