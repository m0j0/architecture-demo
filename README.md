# Доработки по коду

1. создание юзера: кейс с уникальной почтой, отсутствующий Id родителя
2. генератор DU -  переделать на нативный switch
3. автоматизировать генерацию маппингов 
4. DU в сваггере
5. типизированный клиент
5. Guid -> id
6. gRPC

# Доработки по презентации


# Как накатить новую миграцию

В одной консоли:

- cd ArchitectureDemo.IntegrationTests
- docker-compose up

В другой консоли:

- cd ArchitectureDemo.DAL
- dotnet ef migrations add MigrationName

После работы в первой консоли:

docker compose down -v
