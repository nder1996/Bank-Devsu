# 🐳 BancoAPI - Configuración Docker

## Descripción

Esta documentación describe la configuración Docker para **BancoAPI**, un sistema bancario desarrollado en **ASP.NET Core 8.0** que incluye una base de datos **MySQL** y herramientas de desarrollo como **phpMyAdmin**.

## 📋 Prerequisites

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- [Docker](https://docs.docker.com/get-docker/) (versión 20.10 o superior)
- [Docker Compose](https://docs.docker.com/compose/install/) (versión 2.0 o superior)

### Verificar instalación

```bash
docker --version
docker-compose --version
```

## 🚀 Inicio Rápido

### Opción 1: Script Automatizado (Recomendado)

#### En Linux/macOS:
```bash
chmod +x build.sh
./build.sh
```

#### En Windows:
```cmd
build.bat
```

### Opción 2: Comandos Manuales

```bash
# 1. Construir la imagen
docker build -t devsu/banco-api:latest -f BancoAPI/Dockerfile .

# 2. Iniciar todos los servicios
docker-compose up -d

# 3. Verificar estado
docker-compose ps
```

## 📊 Servicios Incluidos

| Servicio | Puerto | Descripción |
|----------|--------|-------------|
| **banco-api** | 8080 | API REST del sistema bancario |
| **mysql-db** | 3306 | Base de datos MySQL 8.0 |
| **phpmyadmin** | 8081 | Administrador web de MySQL |

### URLs de Acceso

- **API BancoAPI**: http://localhost:8080
- **Swagger UI**: http://localhost:8080/swagger
- **phpMyAdmin**: http://localhost:8081
- **MySQL**: localhost:3306

### Credenciales por Defecto

- **MySQL Root**: `root` / `root123`
- **MySQL Usuario**: `bancouser` / `banco123`
- **Base de datos**: `bancodb`

## 📁 Estructura del Proyecto

```
BancoAPI/
├── BancoAPI/
│   ├── Dockerfile              # Imagen optimizada multi-stage
│   └── ...                     # Código fuente de la API
├── docker-compose.yml          # Orquestación de servicios
├── .dockerignore              # Archivos excluidos del build
├── build.sh                   # Script de build para Linux/macOS
├── build.bat                  # Script de build para Windows
└── README-Docker.md           # Esta documentación
```

## 🛠️ Comandos Útiles

### Scripts de Build

```bash
# Construcción completa (limpiar, construir, iniciar)
./build.sh build-and-start

# Solo construir imagen
./build.sh build

# Solo iniciar servicios
./build.sh start

# Ejecutar pruebas
./build.sh test

# Limpiar contenedores e imágenes
./build.sh clean

# Detener servicios
./build.sh stop
```

### Comandos Docker Compose

```bash
# Iniciar servicios en background
docker-compose up -d

# Ver logs en tiempo real
docker-compose logs -f banco-api
docker-compose logs -f mysql-db

# Reiniciar un servicio específico
docker-compose restart banco-api

# Detener todos los servicios
docker-compose down

# Eliminar volúmenes también
docker-compose down -v

# Reconstruir servicios
docker-compose up -d --build
```

### Comandos Docker Directo

```bash
# Ver contenedores activos
docker ps

# Ejecutar comando en contenedor
docker exec -it banco-api bash

# Ver logs de un contenedor
docker logs banco-api -f

# Inspeccionar red
docker network inspect banco-network
```

## 🔧 Configuración

### Variables de Entorno

Las siguientes variables de entorno se pueden modificar en `docker-compose.yml`:

#### MySQL
```yaml
MYSQL_ROOT_PASSWORD: root123
MYSQL_DATABASE: bancodb
MYSQL_USER: bancouser
MYSQL_PASSWORD: banco123
```

#### API
```yaml
ASPNETCORE_ENVIRONMENT: Development
ASPNETCORE_URLS: http://+:8080
ConnectionStrings__DefaultConnection: Server=mysql-db;Database=bancodb;User=bancouser;Password=banco123;
```

### Personalización de Puertos

Para cambiar los puertos, modifica el archivo `docker-compose.yml`:

```yaml
services:
  banco-api:
    ports:
      - "9000:8080"  # Cambiar puerto de la API
  mysql-db:
    ports:
      - "3307:3306"  # Cambiar puerto de MySQL
```

### Volúmenes Persistentes

Los datos de MySQL se almacenan en un volumen persistente:

```yaml
volumes:
  mysql-data:
    driver: local
    name: banco-mysql-data
```

## 🏥 Health Checks

Los servicios incluyen verificaciones de salud:

### API
- **Endpoint**: http://localhost:8080/health
- **Intervalo**: 30 segundos
- **Timeout**: 10 segundos

### MySQL
- **Comando**: `mysqladmin ping`
- **Intervalo**: 10 segundos
- **Timeout**: 20 segundos

## 🐛 Solución de Problemas

### Problemas Comunes

#### 1. Puerto ya en uso
```bash
# Verificar qué proceso usa el puerto
netstat -tulpn | grep :8080

# Cambiar puerto en docker-compose.yml o detener el proceso
```

#### 2. Contenedor no inicia
```bash
# Ver logs detallados
docker-compose logs banco-api

# Verificar estado de salud
docker-compose ps
```

#### 3. Error de conexión a MySQL
```bash
# Verificar que MySQL esté saludable
docker-compose ps mysql-db

# Ver logs de MySQL
docker-compose logs mysql-db

# Reiniciar MySQL
docker-compose restart mysql-db
```

#### 4. Imagen no se construye
```bash
# Limpiar cache de Docker
docker system prune -a

# Reconstruir sin cache
docker-compose build --no-cache
```

### Comandos de Diagnóstico

```bash
# Ver uso de recursos
docker stats

# Verificar redes
docker network ls
docker network inspect banco-network

# Ver volúmenes
docker volume ls
docker volume inspect banco-mysql-data

# Información del sistema Docker
docker system df
docker system info
```

## 🚀 Despliegue en Producción

### 1. Configuración de Producción

Crea un archivo `docker-compose.prod.yml`:

```yaml
version: '3.8'
services:
  banco-api:
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=https://+:443;http://+:80
    ports:
      - "443:443"
      - "80:80"
    volumes:
      - ./certs:/https:ro
```

### 2. Usar archivo de producción

```bash
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

### 3. Configurar SSL/TLS

```bash
# Generar certificados (para desarrollo)
dotnet dev-certs https -ep ./certs/banco-api.pfx -p YourPassword
```

## 📈 Monitoreo

### Logs

```bash
# Ver logs de todos los servicios
docker-compose logs

# Logs con timestamp
docker-compose logs -t

# Seguir logs en tiempo real
docker-compose logs -f --tail=100
```

### Métricas

```bash
# Uso de recursos en tiempo real
docker stats

# Información detallada del contenedor
docker inspect banco-api
```

## 🔄 Actualizaciones

### Actualizar a nueva versión

```bash
# 1. Detener servicios
docker-compose down

# 2. Actualizar código
git pull origin main

# 3. Reconstruir y reiniciar
./build.sh build-and-start
```

### Backup de Base de Datos

```bash
# Crear backup
docker exec banco-mysql mysqldump -u bancouser -p banco123 bancodb > backup.sql

# Restaurar backup
docker exec -i banco-mysql mysql -u bancouser -p banco123 bancodb < backup.sql
```

## 📞 Soporte

Para problemas o preguntas:

1. Revisar los logs: `docker-compose logs`
2. Verificar la documentación de la API
3. Contactar al equipo de desarrollo

---

**Desarrollado por DevSu Bank Team** 🏦