# ⚽ SportsLeague - Sistema de Gestión de Ligas Deportivas

Aplicación backend desarrollada para la gestión de ligas deportivas, permitiendo administrar equipos, torneos y patrocinadores mediante una arquitectura estructurada y escalable.

---

## 📌 Descripción

Este proyecto implementa un sistema backend orientado a la administración de entidades deportivas, diseñado con buenas prácticas de desarrollo para garantizar mantenibilidad, escalabilidad y separación de responsabilidades.

El sistema permite realizar operaciones CRUD sobre las principales entidades del dominio, aplicando patrones de diseño y una arquitectura en capas.

---

## 🚀 Características

- Gestión de equipos
- Gestión de torneos
- Gestión de patrocinadores
- Operaciones CRUD completas
- Validaciones de negocio
- Separación de capas (API, lógica, acceso a datos)
- Código modular y escalable

---

## 🧠 Arquitectura

El proyecto está estructurado bajo una **arquitectura en capas**, separando responsabilidades en:

- API (controladores)
- Lógica de negocio (services)
- Acceso a datos (repository)
- Modelos de dominio

Se implementa el patrón **Repository** para desacoplar la lógica de acceso a datos.

---

## 🛠️ Stack Tecnológico

- C#
- .NET
- Entity Framework
- SQL Server

---

## 🔌 Base de Datos

El sistema utiliza **SQL Server** como motor de base de datos, gestionado mediante **Entity Framework** para el mapeo objeto-relacional (ORM).

---

## ▶️ Ejecución del Proyecto

1. Clonar el repositorio:

```bash
git clone https://github.com/estivencontacto/SportsLeague202601.API-
cd SportsLeague
Configurar la cadena de conexión en:
appsettings.json
Ejecutar el proyecto desde Visual Studio o con:
dotnet run
📊 Funcionalidades Principales
Crear, actualizar, eliminar y consultar equipos
Gestionar torneos y relaciones entre entidades
Administrar patrocinadores
Validar reglas de negocio en la capa de servicios
🔍 Conceptos Aplicados
Arquitectura en capas
Patrón Repository
Entity Framework ORM
Separación de responsabilidades
Buenas prácticas de desarrollo backend
🔧 Posibles Mejoras
Implementación de autenticación (JWT)
Documentación de API con Swagger
Migración a base de datos PostgreSQL
Despliegue en la nube
Pruebas unitarias
👨‍💻 Autor

Estiven Agudelo