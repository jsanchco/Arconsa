Migración
*********

**************
****Previo****
**************
******************************************************************************************************************************************************
******************************************************************************************************************************************************
 - Vistar esta opagina para configurar SQL: https://www.dundas.com/support/learning/documentation/installation/how-to-enable-sql-server-authentication
 - Crear un usuario: user-> admin password-> admin  Luego restart SQL modificar en Security->Logins->admin [entrar en admin y en ServerRoles añadir el check de sysadmin]
 - Restart SQL
******************************************************************************************************************************************************
******************************************************************************************************************************************************

1- Establecer como proyecto de inicio en donde tengamos instanciado SGDE.Domain (cadena de conexión). En mi caso SGDE.API
2- En la 'Consola del Administrador de Paquetes' establecer como 'Proyecto predeterminado' en donde tengamos instanciados los repositorios. En mi caso SGDE.DataEFCoreSQL

DEPENDIENDO del Proyecto (MySQL o SQL)
--------------------------------------
3-   Ejecutamos en Power-Shell 'add-migration First_Migration -Context SGDE.DataEFCoreMySQL.EFContextMySQL'
                               'add-migration First_Migration -Context SGDE.DataEFCoreSQL.EFContextSQL'
4-   Se creará en SGDE.Domain una carpeta en donde se creará la migración
4.1- Establecemos el entorno en el que se va a hacer la migración : $env:ASPNETCORE_ENVIRONMENT='Development'  or  $env:ASPNETCORE_ENVIRONMENT='Production'
5-   Construimos la base de datos en donde haya indicado la cadena de conexión ejecutando en Power-Shell 'Update-Database -Context SGDE.DataEFCoreMySQL.EFContextMySQL'
                                                                                                         'Update-Database -Context SGDE.DataEFCoreSQL.EFContextSQL'
6-   Corremos la app SGDE.SeedData

7-   Si queremos eliminar la Migración, ejecutamos en Power-Shell 'remove-migration -Context SGDE.DataEFCoreMySQL.EFContextMySQL'
                                                                  'remove-migration -Context SGDE.DataEFCoreSQL.EFContextSQL'


Docker
******

Arrancar la imagen del mysql
----------------------------
docker run -d -p 33060:3306 --name mysql-db -e MYSQL_ROOT_PASSWORD=Aceitun@1 --mount src=mysql-db-data,dst=/var/lib/mysql mysql

En el directorio/carpeta de la solucion
----------------------------------------
docker build -f SGDE.API\Dockerfile -t sgde.api .


docker run -p 8000:80 sgde.api

docker-compose up
docker-compose up -d
docker-compose up -d --build

docker-compose down

// Parar/stop el container por nombre
docker stop $(docker ps -q --filter name=mysql)
docker stop $(docker ps -q --filter ancestor=sgde.api)


docker run                  --name mysql-db -e MYSQL_ROOT_PASSWORD=secret -d mysql:tag
docker run -d -p 33060:3306 --name mysql-db -e MYSQL_ROOT_PASSWORD=secret mysqlRESET DOCKER

Explorar Container
------------------
docker exec -t -i mycontainer /bin/bash
docker exec -t -i mycontainer /bin/sh


Limpiar Containers
------------------
docker rm -f $(docker ps -a -q)

Limpiar Volumenes
-----------------
docker volume rm $(docker volume ls -q)

Limpiar Imagenes
----------------
docker rmi $(docker images -a -q)



Remove All tables
*****************

DECLARE @sql NVARCHAR(max)=''

SELECT @sql += ' Drop table ' + QUOTENAME(TABLE_SCHEMA) + '.'+ QUOTENAME(TABLE_NAME) + '; '
FROM   INFORMATION_SCHEMA.TABLES
WHERE  TABLE_TYPE = 'BASE TABLE'

Exec Sp_executesql @sql
GO


