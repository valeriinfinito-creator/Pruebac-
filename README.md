DeportivoApp
Sistema web de gestión deportivo desarrollado en ASP.NET Core MVC, con arquitectura por capas, base de datos MySQL y envío automático de correos.

Descripción
DeportivoApp permite administrar todo el flujo de un complejo deportivo:
Registro de usuarios
Gestión de espacios
Creación y control de espacios/citas
Reportes estadísticos
Notificaciones por correo electrónico

Tecnologías
ASP.NET Core MVC (.NET 8/10)
Entity Framework Core
MySQL
LINQ
Razor Views
SMTP 
Dependency Injection

Arquitectura
Controllers
Services
Models
ViewModels
Data (DbContext)
Helpers (Email, etc)
Validators
Views (Razor)

Funcionalidades
Usuarios
Espacios
Notificaciones
Reportes

Instalación
1. Clonar proyecto
   git clone https://github.com/valeriinfinito-creator/Pruebac-.git

2. Base de datos
   CREATE DATABASE DeportivoDB;

3. Configurar conexión
   "ConnectionStrings": {
   "DefaultConnection": "server=localhost;database=DeportivoDB;user=root;password=1234;"
   }

4. Migraciones
   dotnet ef migrations add InitialCreate
   dotnet ef database update

5. Ejecutar
   dotnet run

Configuración correo
"EmailSettings": {
"SmtpServer": "smtp.gmail.com",
"Port": 587,
"SenderEmail": "correo@gmail.com",
"Password": "app_password"
}

Autor

Valeria Coy Ibarra - Cohorte 6 pm - C#

Estado

Backend completo
Base de datos funcional
Emails automáticos
Reportes implementados
Listo para producción académica

