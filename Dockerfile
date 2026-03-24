# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the project file from the subfolder
COPY ["PillSync/PillSync.csproj", "PillSync/"]
RUN dotnet restore "PillSync/PillSync.csproj"

# Copy everything else
COPY . .

# Move into the project directory
WORKDIR "/src/PillSync"

# Create the empty file to satisfy the compiler
RUN echo "{}" > appsettings.json

RUN dotnet publish "PillSync.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Ensure .NET listens on the port Render uses
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PillSync.dll"]