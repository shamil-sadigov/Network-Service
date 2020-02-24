# Этот приложение, которе по указанному IP адресу возвращает географические координаты. Данные IP адресов автоматически обновляются
## Приложение состоит из двух частей
1. Служба (Hosted Service) - Работающая в отдельном потоке и отвечающая автоматическое  заполнение и обновление базы данных географическими данными полученные из сервиса Maxmind. Обновление происходит по расписанию с использованием Timer класса.
2. Веб-сервис (WebAPI) - по указанному IP адресу возвращает географические координаты

## Предварительные условия (перед запуском приложения)
- Должны быть установлены .NET Core 3.1 SDK, PostgreSQL
- Должны быть указаны опции для подключения к базе данных PostgreSql appsettings.json

![DbOptions](https://raw.githubusercontent.com/ShamilMS/Network-Service/master/IPWebService/Docs/dbOptions.PNG)

- Строку подключения добавлять не надо, строка подключения будет динамически добавляться приложением 
основывась на указанные вами опции для подключенияя

![DbOptions](https://raw.githubusercontent.com/ShamilMS/Network-Service/master/IPWebService/Docs/connString.png)



# Первый запуск
- Так как при первом запуске база данных пустая, то будет выполнен следующий шаг
 - Служба обновления базы данных скачает базу данных Maxmind в формате ".mmdb" и выполнит миграцию всех данных в базу данных PostgreSql
, после чего установит строку подключения к этой базе данных в appsettings.json
 - Так как кол-во данных а базе Maxmind составляет больше миллиарда то миграция может занять значительно время (у меня это составило около 30 минут)
- После этого служба сама будет контролировать актуальность данных и определять когда ей стоит их обновить или нет. 

# Обновление базы данных
 - Служба сама определит актуальность данных и при необходимости скачает свежую базу данных MaxMind и выполнит миграцию в новую созданную базу данных PostgreSQL. После миграции, строка подключения в appsettings.json будет обновлена, указывая на новую базу данных PostgreSQL, а старая база PostgreSQL будет удалена. (Maxmind обновляет свои базу каждый вторник, потому я установил дату обновления каждую среду)
 - На самом деле мы могли бы и не создавать новую базу данных Postgre и обновить существующую, но так как это обновление может занять до 30 минут а то и больше, то веб сервис не сможет отвечать на запросы так как база данных обновляется. Соответственно  создание новой базы данных Postgre не препятствует работе веб-сервиса, а после создания старая база данных удаляется и все последующие запросы будут направлены к новой базе данных Postgre.

