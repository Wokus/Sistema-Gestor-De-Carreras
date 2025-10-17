# Sistema gestor de Carreras

Prueba de concepto hecha para la materia de Taller de .NET del Tecnólogo en Informática en el CURE.


[Documentación oficial](https://docs.google.com/document/d/1pJRYvKORJx-PAIt4gDxRtVg4F3dO-YIm7tRdVfwRwSc/edit?usp=sharing)


---
# Como usar

## Base de datos

	"dotnet ef migrations add NombreDeImagen"

Crea la migracion de la base de datos con el nombre "nombredeimagen".
Cada vez que se hagan cambios en la bd hay que crear una migracion nueva.

	dotnet ef database update

Aplica la migracion

Para poder usar los comandos de dotnet ef hay que instalarlo.  

	dotnet tool install --global dotnet-ef

