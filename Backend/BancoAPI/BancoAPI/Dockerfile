# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /src
COPY BancoAPI/BancoAPI.csproj ./BancoAPI/
RUN dotnet restore ./BancoAPI/BancoAPI.csproj

COPY . .
WORKDIR /src/BancoAPI
RUN dotnet publish -c Release -o /app/publish --no-restore

# Etapa final - Imagen de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Crear usuario no-root
RUN adduser -D -u 1001 appuser

# Instalar dependencias básicas
RUN apk add --no-cache curl

# Configuración básica
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080 \
    TZ=America/Bogota

WORKDIR /app
EXPOSE 8080

# Copiar archivos publicados
COPY --from=build --chown=appuser:1001 /app/publish .

# Usar usuario sin privilegios
USER 1001

# Health check básico
HEALTHCHECK --interval=30s --timeout=10s --start-period=30s --retries=3 \
    CMD curl -f http://localhost:8080/api/clientes || exit 1

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "BancoAPI.dll"]