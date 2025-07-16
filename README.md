# AppCapasCitas - Sistema de Gestión de Citas Médicas

## 📋 Descripción
Sistema completo de gestión de citas médicas desarrollado con Clean Architecture, implementando CQRS, Repository Pattern y generación de reportes PDF.

## 🏗️ Arquitectura
- **Frontend**: Blazor WebAssembly (.NET 9.0)
- **Backend**: ASP.NET Core Web API (.NET 9.0)
- **Base de Datos**: SQL Server
- **Autenticación**: JWT (JSON Web Tokens)
- **Reportes**: iText7 (PDF), ZIP compression
- **Logging**: HTML File Logger

## 📁 Estructura del Proyecto

```
AppCapasCitas/
├── AppCapasCitas.API/              # Web API (Controllers, Swagger)
├── AppCapasCitas.Application/      # Capa de Aplicación (CQRS, Handlers)
├── AppCapasCitas.Domain/           # Capa de Dominio (Entities, Models)
├── AppCapasCitas.DTO/              # Data Transfer Objects
├── AppCapasCitas.FrontEnd/         # Blazor WebAssembly
├── AppCapasCitas.Identity/         # Autenticación y Autorización
├── AppCapasCitas.Infrastructure/   # Persistencia de Datos (EF Core)
├── AppCapasCitas.Reporting/        # Generación de Reportes PDF
└── AppCapasCitas.Transversal.*/    # Servicios Transversales
```

## 🚀 Características Principales

### 👥 Gestión de Usuarios
- Autenticación JWT
- Roles y permisos
- Gestión de médicos y pacientes

### 📊 Reportes Avanzados
- Expedientes médicos individuales
- Generación múltiple con compresión ZIP
- PDF personalizables con marcas de agua
- Configuración de diseño y formato

### 🎨 Frontend Moderno
- Blazor WebAssembly con Bootstrap
- Componentes reutilizables
- Notificaciones Toast y SweetAlert
- Selección múltiple optimizada

### 🔧 Backend Robusto
- Clean Architecture
- CQRS con MediatR
- Repository Pattern
- Entity Framework Core

## 🛠️ Tecnologías Utilizadas

### Frontend
- Blazor WebAssembly
- Bootstrap 5
- Blazored.Toast
- SweetAlert2
- Blazored.SessionStorage

### Backend
- ASP.NET Core 9.0
- Entity Framework Core
- MediatR (CQRS)
- JWT Authentication
- Swagger/OpenAPI

### Reportes
- iText7 (PDF generation)
- System.IO.Compression (ZIP)
- HTML to PDF conversion

## ⚙️ Configuración e Instalación

### Prerrequisitos
- .NET 9.0 SDK
- SQL Server 2019+
- Visual Studio 2022 o VS Code

### 1. Clonar el repositorio
```bash
git clone https://github.com/erviquez/AppCapasCitas.git
cd AppCapasCitas
```

### 2. Configurar Base de Datos
```bash
# Actualizar connection string en appsettings.json
# Ejecutar migraciones
dotnet ef database update --project AppCapasCitas.Infrastructure
```

### 3. Ejecutar la aplicación
```bash
# Backend API
dotnet run --project AppCapasCitas.API

# Frontend (en otra terminal)
dotnet run --project AppCapasCitas.FrontEnd
```

## 📝 Uso del Sistema

### Autenticación
1. Acceder a la aplicación
2. Iniciar sesión con credenciales válidas
3. El sistema generará un token JWT

### Gestión de Médicos
1. Navegar a "Médicos"
2. Seleccionar médicos usando checkboxes
3. Generar expedientes individuales o múltiples
4. Descargar archivos PDF o ZIP

### Reportes
- **Individual**: Un expediente por médico
- **Múltiple**: ZIP con todos los expedientes seleccionados
- **Personalizable**: Configuración de diseño y contenido

## 🤝 Contribución
1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📄 Licencia
Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.

## 👨‍💻 Autor
**Ernesto De la O**
- Email: erviquez@gmail.com
- GitHub: [@erviquez](https://github.com/erviquez)

## 🔄 Versión
- **v1.0.0** - Versión inicial con gestión básica
- **v1.1.0** - Reportes PDF y expedientes
- **v1.2.0** - Generación múltiple con ZIP