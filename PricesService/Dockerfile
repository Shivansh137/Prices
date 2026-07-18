# Stage 1: Build the binary inside an SDK container environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies to exploit layer caching optimizations
COPY ["PricesService.csproj", "."]
RUN dotnet restore

# Copy all files and compile binaries for production release
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Create runtime container image with stripped down dependency footprint
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "PricesService.dll"]