# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copy the project file (Relative to your build context)
# If you run 'docker build' from the Root, use "PillSync/"
COPY ["PillSync/PillSync.csproj", "PillSync/"]

# 2. Restore using the path inside the container
RUN dotnet restore "PillSync/PillSync.csproj"

# 3. Copy the rest of the source code
COPY . .

# 4. Move to the project directory to publish
WORKDIR "/src/PillSync"

# Create a dummy appsettings if needed for the build
RUN echo "{}" > appsettings.json

RUN dotnet publish "PillSync.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Render often uses 8080 or 10000; ensure this matches your dashboard
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PillSync.dll"]