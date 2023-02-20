# Доработки по коду

1. создание юзера: кейс с уникальной почтой, отсутствующий Id родителя
2. Guid -> id
3. генератор DU -  переделать на нативный switch
4. автоматизировать генерацию маппингов 
5. DU в сваггере
6. типизированный клиент
7. gRPC
8. тесты в докере на CI

# Доработки по презентации

1. TODO

# Как накатить новую миграцию

В одной консоли:

- cd ArchitectureDemo.IntegrationTests
- docker-compose up

В другой консоли:

- cd ArchitectureDemo.DAL
- dotnet ef migrations add MigrationName

После работы в первой консоли:

- docker compose down -v
