# О репозитории

TODO

# Доработки по коду

1. gRPC для файлов
2. Теряются файлы паролей пг в тестах
3. Доработать анализатор размеченных определений

# Как накатить новую миграцию

В одной консоли:

- cd ArchitectureDemo.IntegrationTests
- docker-compose up

В другой консоли:

- cd ArchitectureDemo.DAL
- dotnet ef migrations add MigrationName

После работы в первой консоли:

- docker compose down -v
