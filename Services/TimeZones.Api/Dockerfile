FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Services/TimeZones.Api/TimeZones.Api.csproj Services/TimeZones.Api/
RUN dotnet restore Services/TimeZones.Api/TimeZones.Api.csproj
COPY . .
WORKDIR /src/Services/TimeZones.Api
RUN dotnet build TimeZones.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TimeZones.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TimeZones.Api.dll"]
