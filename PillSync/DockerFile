# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copy csproj and restore to cache layers
COPY ["PillSync.csproj", "./"] 
RUN dotnet restore

# 2. Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# 3. Tell .NET to listen on 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

# Render defaults to 10000, but 8080 is the .NET 8+ default
EXPOSE 8080

ENTRYPOINT ["dotnet", "PillSync.dll"]
