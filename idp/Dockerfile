FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY idp/idp.csproj idp/
RUN dotnet restore idp/idp.csproj
COPY . .
WORKDIR /src/idp
RUN dotnet build idp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish idp.csproj -c Release -o /app
RUN cd /app && mkdir persistances && mkdir keys

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN cd / && mkdir keys && mkdir db
ENTRYPOINT ["dotnet", "idp.dll"]
