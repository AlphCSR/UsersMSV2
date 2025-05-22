# Ejecución de Pruebas Unitarias y Reporte de Cobertura

Este archivo describe los comandos necesarios para ejecutar las pruebas unitarias y obtener el reporte de cobertura de código en este proyecto .NET.

## 1. Restaurar dependencias

Ejecuta el siguiente comando para restaurar los paquetes NuGet necesarios:

```sh
cd UsersMS.Test

dotnet restore
```

## 2. Ejecutar las pruebas unitarias y obtener el reporte de cobertura

Ejecuta el siguiente comando para correr las pruebas y generar el reporte de cobertura en formato Cobertura XML:

```sh
dotnet test --collect:"XPlat Code Coverage"
```

Esto generará un archivo de cobertura en la carpeta `TestResults` dentro de `UsersMS.Test`, por ejemplo:

```
UsersMS.Test\TestResults\<GUID>\coverage.cobertura.xml
```

## 3. Visualizar el reporte de cobertura

Puedes abrir el archivo `coverage.cobertura.xml` con herramientas como:
- [ReportGenerator](https://danielpalme.github.io/ReportGenerator/)
- Extensiones de Visual Studio
- Visores online de Cobertura XML

```sh
reportgenerator -reports:"c:\Users\cesar\Downloads\Desarrollo\UsersMS\UsersMS.Test\TestResults\*\coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```

---

**Notas:**
- Si agregas o modificas pruebas, vuelve a ejecutar los comandos anteriores para actualizar el reporte.
- Si tienes errores de detección de pruebas, asegúrate de tener los paquetes `xunit.runner.visualstudio` y `coverlet.collector` instalados en tu archivo `.csproj` de tests.
