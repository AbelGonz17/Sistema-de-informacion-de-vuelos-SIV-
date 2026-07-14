# Sistema de Información de Vuelos (SIV) ✈️

¡Bienvenido al repositorio del **Sistema de Información de Vuelos (SIV)**! Este proyecto está desarrollado utilizando **C# .NET** y sigue los principios de **Clean Architecture** (Arquitectura Limpia), garantizando un código mantenible, escalable y fácil de probar.

## 🏗️ Arquitectura del Proyecto

El proyecto está dividido en múltiples capas para mantener una clara separación de responsabilidades:

- **SIV.Domain**: Contiene las entidades principales, interfaces y reglas de negocio del dominio.
- **SIV.Application**: Maneja los casos de uso, lógica de la aplicación y contratos de los servicios.
- **SIV.Infrastructure**: Implementación de servicios externos y utilidades.
- **SIV.Persistence**: Configuración de la base de datos (Entity Framework Core), repositorios y migraciones.
- **SIV.IOC** (Inversion of Control): Configuración de inyección de dependencias centralizada.
- **SIV.Api**: API RESTful que sirve como punto de entrada para servicios backend.
- **SIV.Web**: Aplicación web frontend/UI.
- **Pruebas (Tests)**: Proyectos como `SIV.Application.Test` y `SIV.Persistence.Test` para garantizar la calidad del código mediante pruebas unitarias.

## 🚀 Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (o superior)
- SQL Server u otro motor de base de datos compatible configurado.
- Docker (opcional, para entornos en contenedores).

## 🛠️ Configuración y Ejecución

1. **Clonar el repositorio:**
   ```bash
   git clone <url-del-repositorio>
   cd SIV
   ```

2. **Configuración de base de datos:**
   Asegúrate de configurar la cadena de conexión en el archivo `appsettings.json` o en tu archivo `.env`.

3. **Aplicar migraciones:**
   ```bash
   dotnet ef database update --project SIV.Persistence --startup-project SIV.Api
   ```

4. **Ejecutar el proyecto API:**
   ```bash
   cd SIV.Api
   dotnet run
   ```

5. **Ejecutar el proyecto Web:**
   ```bash
   cd SIV.Web
   dotnet run
   ```

## 🐳 Docker

El proyecto incluye un archivo `docker-compose.yml` para facilitar el despliegue del entorno con contenedores.
Para ejecutar usando Docker:
```bash
docker-compose up -d
```

## 📄 Documentación
Puedes encontrar las especificaciones y requisitos del sistema en el archivo `SRS_SIV_v1.0.pdf` incluido en el directorio raíz.
