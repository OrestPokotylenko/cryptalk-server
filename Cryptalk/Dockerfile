# --- Base runtime for the API ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Cryptalk/Cryptalk.csproj Cryptalk/
RUN dotnet restore Cryptalk/Cryptalk.csproj

COPY . .

WORKDIR /src/Cryptalk
RUN dotnet build Cryptalk.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
WORKDIR /src/Cryptalk
RUN dotnet publish Cryptalk.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final API image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cryptalk.dll"]

# Migration runner
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final-migration
WORKDIR /src

COPY Cryptalk/Cryptalk.csproj Cryptalk/
RUN dotnet restore Cryptalk/Cryptalk.csproj

COPY . .

COPY .config/dotnet-tools.json .config/dotnet-tools.json
RUN dotnet tool restore

COPY migrate.sh /migrate.sh
RUN chmod +x /migrate.sh
ENTRYPOINT ["/migrate.sh"]
