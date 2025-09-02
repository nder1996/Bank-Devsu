#!/bin/bash

# Script de construcción y despliegue para BancoAPI
# Autor: DevSu Bank Team
# Descripción: Automatiza la construcción, pruebas y despliegue de la aplicación

set -e  # Salir en caso de error

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Variables
PROJECT_NAME="banco-api"
IMAGE_NAME="devsu/banco-api"
VERSION="${1:-latest}"
DOCKER_COMPOSE_FILE="docker-compose.yml"

# Funciones
print_header() {
    echo -e "${BLUE}=================================="
    echo -e "     DevSu Bank - Build Script"
    echo -e "==================================${NC}"
    echo ""
}

print_step() {
    echo -e "${YELLOW}[STEP] $1${NC}"
}

print_success() {
    echo -e "${GREEN}[SUCCESS] $1${NC}"
}

print_error() {
    echo -e "${RED}[ERROR] $1${NC}"
}

# Verificar prerequisites
check_prerequisites() {
    print_step "Verificando prerequisites..."
    
    if ! command -v docker &> /dev/null; then
        print_error "Docker no está instalado"
        exit 1
    fi
    
    if ! command -v docker-compose &> /dev/null; then
        print_error "Docker Compose no está instalado"
        exit 1
    fi
    
    print_success "Prerequisites verificados correctamente"
}

# Limpiar contenedores e imágenes anteriores
clean_up() {
    print_step "Limpiando contenedores e imágenes anteriores..."
    
    # Detener y eliminar contenedores
    docker-compose down --remove-orphans 2>/dev/null || true
    
    # Eliminar imágenes anteriores del proyecto
    docker images "$IMAGE_NAME" --format "table {{.Repository}}\t{{.Tag}}\t{{.ID}}" | grep -v REPOSITORY | awk '{print $3}' | xargs -r docker rmi -f 2>/dev/null || true
    
    # Limpiar imágenes huérfanas
    docker image prune -f > /dev/null 2>&1 || true
    
    print_success "Limpieza completada"
}

# Construir la imagen Docker
build_image() {
    print_step "Construyendo imagen Docker..."
    
    docker build \
        --target final \
        --tag "$IMAGE_NAME:$VERSION" \
        --tag "$IMAGE_NAME:latest" \
        --build-arg BUILD_CONFIGURATION=Release \
        -f BancoAPI/Dockerfile \
        .
    
    print_success "Imagen construida: $IMAGE_NAME:$VERSION"
}

# Ejecutar pruebas en contenedor
run_tests() {
    print_step "Ejecutando pruebas..."
    
    # Construir imagen de pruebas
    docker build \
        --target build \
        --tag "$IMAGE_NAME:test" \
        -f BancoAPI/Dockerfile \
        .
    
    # Ejecutar pruebas
    docker run --rm \
        -v "$(pwd):/src" \
        "$IMAGE_NAME:test" \
        sh -c "cd /src/BancoAPI && dotnet test --no-build --verbosity normal"
    
    print_success "Pruebas ejecutadas correctamente"
}

# Iniciar servicios con docker-compose
start_services() {
    print_step "Iniciando servicios con Docker Compose..."
    
    # Verificar que existe docker-compose.yml
    if [ ! -f "$DOCKER_COMPOSE_FILE" ]; then
        print_error "Archivo $DOCKER_COMPOSE_FILE no encontrado"
        exit 1
    fi
    
    # Iniciar servicios
    docker-compose up -d
    
    print_success "Servicios iniciados correctamente"
}

# Verificar estado de los servicios
check_health() {
    print_step "Verificando estado de los servicios..."
    
    sleep 30  # Esperar a que los servicios inicien
    
    # Verificar MySQL
    if docker-compose ps mysql-db | grep -q "healthy"; then
        print_success "MySQL está saludable"
    else
        print_error "MySQL no está saludable"
    fi
    
    # Verificar API
    if docker-compose ps banco-api | grep -q "healthy"; then
        print_success "BancoAPI está saludable"
    else
        print_error "BancoAPI no está saludable"
    fi
    
    echo ""
    echo -e "${BLUE}=== Estado de los servicios ===${NC}"
    docker-compose ps
}

# Mostrar información de acceso
show_access_info() {
    echo ""
    echo -e "${GREEN}=== Información de Acceso ===${NC}"
    echo -e "${YELLOW}API BancoAPI:${NC}     http://localhost:8080"
    echo -e "${YELLOW}Swagger UI:${NC}       http://localhost:8080/swagger"
    echo -e "${YELLOW}phpMyAdmin:${NC}       http://localhost:8081"
    echo -e "${YELLOW}MySQL:${NC}            localhost:3306 (bancouser/banco123)"
    echo ""
    echo -e "${BLUE}=== Comandos útiles ===${NC}"
    echo "Ver logs API:         docker-compose logs -f banco-api"
    echo "Ver logs MySQL:       docker-compose logs -f mysql-db"
    echo "Detener servicios:    docker-compose down"
    echo "Reiniciar API:        docker-compose restart banco-api"
    echo ""
}

# Función principal
main() {
    print_header
    
    case "${1:-build-and-start}" in
        "clean")
            clean_up
            ;;
        "build")
            check_prerequisites
            build_image
            ;;
        "test")
            check_prerequisites
            run_tests
            ;;
        "start")
            start_services
            check_health
            show_access_info
            ;;
        "build-and-start"|"")
            check_prerequisites
            clean_up
            build_image
            # run_tests  # Descomentar cuando existan pruebas
            start_services
            check_health
            show_access_info
            ;;
        "stop")
            print_step "Deteniendo servicios..."
            docker-compose down
            print_success "Servicios detenidos"
            ;;
        *)
            echo "Uso: $0 [clean|build|test|start|build-and-start|stop]"
            echo ""
            echo "Comandos:"
            echo "  clean            - Limpiar contenedores e imágenes"
            echo "  build            - Solo construir imagen"
            echo "  test             - Ejecutar pruebas"
            echo "  start            - Solo iniciar servicios"
            echo "  build-and-start  - Construir e iniciar (por defecto)"
            echo "  stop             - Detener servicios"
            exit 1
            ;;
    esac
}

# Ejecutar función principal
main "$@"