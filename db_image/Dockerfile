# db_image/Dockerfile
FROM mysql:8.0
COPY SchemaMedSync.sql /docker-entrypoint-initdb.d/
ENV MYSQL_ROOT_PASSWORD=123456 
ENV MYSQL_DATABASE=medsync_db     