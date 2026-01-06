# # Base image untuk build (.NET 9 SDK stable)
# FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# WORKDIR /src

# # Salin file .csproj dan restore dependencies
# COPY *.csproj ./
# RUN dotnet restore

# # Salin semua source code dan publish
# COPY . .
# RUN dotnet publish -c Release -o /app/publish

# # Runtime image (.NET 9 ASP.NET runtime stable)
# FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
# WORKDIR /app
# COPY --from=build /app/publish .

# ENTRYPOINT ["dotnet", "HamatetsuScheduler.dll"]

# ==========================
# 🔧 Build Stage
# ==========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution & project files
COPY HamatetsuScheduler.sln ./
COPY HamatetsuScheduler.Api/*.csproj ./HamatetsuScheduler.Api/

# Restore dependencies dari solution
RUN dotnet restore HamatetsuScheduler.sln

# Copy semua source code
COPY . .

# Publish project WebApi
WORKDIR /src/HamatetsuScheduler.Api
RUN dotnet publish -c Release -o /app/publish

# ==========================
# 🚀 Runtime Stage
# ==========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "HamatetsuScheduler.Api.dll"]