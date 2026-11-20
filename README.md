# DesAEA13 - Laboratorio 13 Code First

Aplicación ASP.NET Core MVC (.NET 8) con Entity Framework Core y SQL Server usando autenticación de Windows.

## Modelo

Grade, Course, Student y Enrollment. Grade y Course usan eliminación lógica con el campo `Active`.

## Requisitos

- .NET 8 SDK
- SQL Server Express con la instancia `.\SQLEXPRESS` (se cambia en `appsettings.json`)
- dotnet-ef 8.0.11

## Ejecución

```
dotnet restore
dotnet ef database update
dotnet run
```

## Pantallas

- `/Grades` CRUD de grados
- `/Courses` CRUD de cursos
- `/Students` CRUD de estudiantes
- `/Enrollments` matrículas

## Endpoints

- `GET /api/students/search?firstName=&lastName=&email=`
- `GET /api/students/search-by-grade?firstName=&grade=`
- `GET /api/enrollments/by-course?course=`
- `GET /api/enrollments/by-grade?grade=`
