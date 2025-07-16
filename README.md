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
git clone https://github.com/tu-usuario/AppCapasCitas.git
cd AppCapasCitas