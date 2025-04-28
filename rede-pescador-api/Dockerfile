# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copia a aplicação publicada
COPY --from=build /app/out .

# Copia a pasta de scripts de migração também
COPY --from=build /app/db ./db


ENTRYPOINT ["dotnet", "rede-pescador-api.dll"]
