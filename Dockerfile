# ==========================================
# TRINN 1: Sett opp kjøremiljøet (Runtime)
# ==========================================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# ==========================================
# TRINN 2: Gjenopprett og bygg applikasjonen
# ==========================================

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Kopier prosjektfilen først for å utnytte caching av NuGet-pakker

COPY ["PathFinder.Api/PathFinder.Api.csproj", "PathFinder.Api/"]
COPY ["PathFinder/PathFinder.csproj", "PathFinder/"]
RUN dotnet restore "PathFinder.Api/PathFinder.Api.csproj"

# Kopier resten av kildekoden og bygg prosjektet i Release-modus

COPY . .
WORKDIR "/src/PathFinder.Api"
RUN dotnet build "PathFinder.Api.csproj" -c Release -o /app/build

# ==========================================
# TRINN 3: Klargjør filene for publisering
# ==========================================

FROM build AS publish
RUN dotnet publish "PathFinder.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==========================================
# TRINN 4: Sett sammen den endelige containeren
# ==========================================

FROM base AS final
WORKDIR /app
# Kopierer kun de kompilerte filene fra trinn 3 (ingen kildekode eller SDK blir med videre)
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PathFinder.Api.dll"]
