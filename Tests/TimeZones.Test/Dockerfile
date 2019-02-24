FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Tests/TimeZones.Test/TimeZones.Test.csproj Tests/TimeZones.Test/
COPY Services/TimeZones.Service/TimeZones.Service.csproj Services/TimeZones.Service/
COPY Services/TimeZones.Extensibility/TimeZones.Extensibility.csproj Services/TimeZones.Extensibility/
RUN dotnet restore Tests/TimeZones.Test/TimeZones.Test.csproj
COPY . .
WORKDIR /src/Tests/TimeZones.Test
RUN dotnet build TimeZones.Test.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TimeZones.Test.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TimeZones.Test.dll"]
