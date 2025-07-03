# --- Etapa de Build ---
# Usamos la imagen del SDK de .NET 9 para compilar el proyecto.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos los archivos del proyecto y restauramos las dependencias.
COPY ["EducationalPlatformApi.csproj", "./"]
RUN dotnet restore "EducationalPlatformApi.csproj"

# Copiamos el resto del código fuente y construimos la aplicación.
COPY . .
WORKDIR "/src/."
RUN dotnet build "EducationalPlatformApi.csproj" -c Release -o /app/build

# --- Etapa de Publicación ---
# Generamos los archivos listos para producción.
FROM build AS publish
RUN dotnet publish "EducationalPlatformApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Etapa Final ---
# Usamos una imagen ligera de ASP.NET para ejecutar la aplicación.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiamos los archivos publicados desde la etapa anterior.
COPY --from=publish /app/publish .

# Exponemos el puerto 80 (Render se encargará del mapeo externamente).
EXPOSE 80

# El comando para iniciar la aplicación cuando el contenedor se ejecute.
ENTRYPOINT ["dotnet", "EducationalPlatformApi.dll"]