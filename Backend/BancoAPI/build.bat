@echo off
REM Script de construcción y despliegue para BancoAPI (Windows)
REM Autor: DevSu Bank Team
REM Descripción: Automatiza la construcción, pruebas y despliegue de la aplicación

setlocal enabledelayedexpansion

REM Variables
set PROJECT_NAME=banco-api
set IMAGE_NAME=devsu/banco-api
set VERSION=%1
if "%VERSION%"=="" set VERSION=latest
set DOCKER_COMPOSE_FILE=docker-compose.yml

REM Colores (usando echo con códigos de escape cuando sea posible)
set RED=[31m
set GREEN=[32m
set YELLOW=[33m
set BLUE=[34m
set NC=[0m

goto main

:print_header
echo ==================================
echo      DevSu Bank - Build Script
echo ==================================
echo.
goto :eof

:print_step
echo [STEP] %~1
goto :eof

:print_success
echo [SUCCESS] %~1
goto :eof

:print_error
echo [ERROR] %~1
goto :eof

:check_prerequisites
call :print_step "Verificando prerequisites..."

docker --version >nul 2>&1
if errorlevel 1 (
    call :print_error "Docker no está instalado"
    exit /b 1
)

docker-compose --version >nul 2>&1
if errorlevel 1 (
    call :print_error "Docker Compose no está instalado"
    exit /b 1
)

call :print_success "Prerequisites verificados correctamente"
goto :eof

:clean_up
call :print_step "Limpiando contenedores e imágenes anteriores..."

REM Detener y eliminar contenedores
docker-compose down --remove-orphans >nul 2>&1

REM Eliminar imágenes anteriores del proyecto
for /f "tokens=3" %%i in ('docker images %IMAGE_NAME% --format "table {{.Repository}}\t{{.Tag}}\t{{.ID}}" ^| findstr /v REPOSITORY') do (
    docker rmi -f %%i >nul 2>&1
)

REM Limpiar imágenes huérfanas
docker image prune -f >nul 2>&1

call :print_success "Limpieza completada"
goto :eof

:build_image
call :print_step "Construyendo imagen Docker..."

docker build --target final --tag "%IMAGE_NAME%:%VERSION%" --tag "%IMAGE_NAME%:latest" --build-arg BUILD_CONFIGURATION=Release -f BancoAPI/Dockerfile .

if errorlevel 1 (
    call :print_error "Error construyendo la imagen"
    exit /b 1
)

call :print_success "Imagen construida: %IMAGE_NAME%:%VERSION%"
goto :eof

:run_tests
call :print_step "Ejecutando pruebas..."

REM Construir imagen de pruebas
docker build --target build --tag "%IMAGE_NAME%:test" -f BancoAPI/Dockerfile .

if errorlevel 1 (
    call :print_error "Error construyendo imagen de pruebas"
    exit /b 1
)

REM Ejecutar pruebas
docker run --rm -v "%cd%:/src" "%IMAGE_NAME%:test" sh -c "cd /src/BancoAPI && dotnet test --no-build --verbosity normal"

if errorlevel 1 (
    call :print_error "Error ejecutando pruebas"
    exit /b 1
)

call :print_success "Pruebas ejecutadas correctamente"
goto :eof

:start_services
call :print_step "Iniciando servicios con Docker Compose..."

if not exist "%DOCKER_COMPOSE_FILE%" (
    call :print_error "Archivo %DOCKER_COMPOSE_FILE% no encontrado"
    exit /b 1
)

docker-compose up -d

if errorlevel 1 (
    call :print_error "Error iniciando servicios"
    exit /b 1
)

call :print_success "Servicios iniciados correctamente"
goto :eof

:check_health
call :print_step "Verificando estado de los servicios..."

timeout /t 30 >nul

REM Verificar estado de los servicios
docker-compose ps mysql-db | findstr "healthy" >nul
if not errorlevel 1 (
    call :print_success "MySQL está saludable"
) else (
    call :print_error "MySQL no está saludable"
)

docker-compose ps banco-api | findstr "healthy" >nul
if not errorlevel 1 (
    call :print_success "BancoAPI está saludable"
) else (
    call :print_error "BancoAPI no está saludable"
)

echo.
echo === Estado de los servicios ===
docker-compose ps
goto :eof

:show_access_info
echo.
echo === Información de Acceso ===
echo API BancoAPI:     http://localhost:8080
echo Swagger UI:       http://localhost:8080/swagger
echo phpMyAdmin:       http://localhost:8081
echo MySQL:            localhost:3306 (bancouser/banco123)
echo.
echo === Comandos útiles ===
echo Ver logs API:         docker-compose logs -f banco-api
echo Ver logs MySQL:       docker-compose logs -f mysql-db
echo Detener servicios:    docker-compose down
echo Reiniciar API:        docker-compose restart banco-api
echo.
goto :eof

:main
call :print_header

set COMMAND=%1
if "%COMMAND%"=="" set COMMAND=build-and-start

if "%COMMAND%"=="clean" (
    call :clean_up
) else if "%COMMAND%"=="build" (
    call :check_prerequisites
    call :build_image
) else if "%COMMAND%"=="test" (
    call :check_prerequisites
    call :run_tests
) else if "%COMMAND%"=="start" (
    call :start_services
    call :check_health
    call :show_access_info
) else if "%COMMAND%"=="build-and-start" (
    call :check_prerequisites
    call :clean_up
    call :build_image
    REM call :run_tests  REM Descomentar cuando existan pruebas
    call :start_services
    call :check_health
    call :show_access_info
) else if "%COMMAND%"=="stop" (
    call :print_step "Deteniendo servicios..."
    docker-compose down
    call :print_success "Servicios detenidos"
) else (
    echo Uso: %0 [clean^|build^|test^|start^|build-and-start^|stop]
    echo.
    echo Comandos:
    echo   clean            - Limpiar contenedores e imágenes
    echo   build            - Solo construir imagen
    echo   test             - Ejecutar pruebas
    echo   start            - Solo iniciar servicios
    echo   build-and-start  - Construir e iniciar (por defecto)
    echo   stop             - Detener servicios
    exit /b 1
)

endlocal