# ProductCatalog

This is a part of Curotec Technical Assessment


This project was created with JetBrains Rider 2024.3.6  Build #RD-243.25659.34, built on February 26, 2025

to run the project, you can run from VisualStudio also, just put the webAPI project to default.

you can also run by the dotnet run command (called from ProductCatalog/src/webAPI/webAPI)

The project will start on the below url:

 
   http://localhost:5086/swagger/index.html


TradeOffs / Description / Disclaimer 

Its a webAPI that runs with .net 7.0 (as requested on the Technical Assessment Description)

The proposal was to develop one webAPI for implementing a Product Catalog in 2 hours using EF as ORM for Database.

In my opinion and experience, the Database is just a repo for data, the Domain should be as Rich as possible and i always leave the Repo to the Last part (and usually we 
have already one database, EF is very good for new greenfield projects)

For saking of Time and previous conversation with Gabriel that explain me:
"The goal is to demonstrate/mock the functions and what is being asked in the user stories given to you fitting the maximum amount of technical requirements list you can"

I created the webAPI with Products and Categories Controller using a FakeRepository (implemented from contracts)
All repos and Controllers for webAPI are tested, so the Database could be any (as long it uses the same contracts - which is very simple to implement)

Also, because of time, i didn't create one Rich Domain (using Value Objects and other DDD Validations - also because of time)

I couldn't implement Cache also. But maybe with more 1 hour is totally possible to implement EF and caching.

But there is one implementation for BackgroundServices (also, the best approach is to process messages from ExternalSources - Like RabbitMQ or other)
The example implemented is just to show the implementation for BackGroundServices (introduced on .net 6 - if i'm not mistaken)

The approach for database that i use is to create a docker-compose file with the System (MSSQL, Oracle, Postgres, MySQL, etc) and connect with ADO and Dapper, so this also takes some time.

If i forgot anything, please let me know and i will be very happy to explain

Thank you so much in advance
