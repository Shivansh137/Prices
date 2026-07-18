FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore via the solution file
COPY . .
RUN dotnet restore Prices.sln

# ==========================================
# CI STAGE: Run unit tests
# ==========================================
FROM build AS test
# If this fails, the entire pipeline stops immediately.
RUN dotnet test Tests/Tests.csproj -c Release

# ==========================================
# CD STAGE: Publish the compiled API
# ==========================================
FROM build AS publish
RUN dotnet publish PricesService.csproj -c Release -o /app/publish

# ==========================================
# FINAL RUNTIME STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PricesService.dll"]