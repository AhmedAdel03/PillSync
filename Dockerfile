# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# FIX: Point to the subfolder for the csproj
COPY ["PillSync/PillSync.csproj", "PillSync/"]
RUN dotnet restore "PillSync/PillSync.csproj"

# Copy everything else
COPY . .

# FIX: Move into the project directory before publishing
WORKDIR "/src/PillSync"
RUN echo "{}" > PillSync/appsettings.json
RUN dotnet publish "PillSync.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Ensure .NET listens on the port Render uses
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PillSync.dll"]