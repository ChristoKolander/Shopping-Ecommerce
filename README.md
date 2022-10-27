# Shopping Demo Lab

A)

This is a private Repo... 

Testsite - trying out different approaches and techniques!

Blazor Web Assembly upfront.

Code is not for production. 

Just lab.

Recently upgraded from 3.1 to .Net 6.

Order implementation the final piece, going to use CQRS pattern for that. Hence tha misleading folder name.Soon.

B)

If you want to try out this code live - instead of downloading - go to: NotImplementedYet!

C)

If you want to download and test the code you will have to use Visual Studio 2022:

1. Download, extract, rebuild to check everything is downloaded.

2. You might have to run these commands - and dont forget to adjust SqlServer Connection settings if so (and to remove old migrations folder first!).

Adjusting settings done via file Dependencies inside Project Shopping.Infrastructure and AppSettings inside Project Shopping.Api

Commands if needed: 

Add-Migration InitialProd -Context ProductContext -StartupProject Shopping.Api -OutputDir Migrations/Prod

Update-Database -Context ProductContext -StartupProject Shopping.Api

Add-Migration InitialAppId -Context AppIdentityDbContext -StartupProject Shopping.Api -OutputDir Migrations/AppId

Update-Database -Context AppIdentityDbContext -StartupProject Shopping.Api


3. Set as multiple start-up projects from solution explorer, API on top and Shopping.Web.Portal below.
4. There are 2 initial Users seeded, but u can try registering one, TestUser@hotmail.com for instance.
5. Ctrl F5.
6. Note: if you register as a new user in Development, you will be given "Administrator" role by default. Change this to "standardUser" in AccountController/Registration,
   or something else(see db for all roles seeded).
   
Finally: Hard to find material on how to implement a disconnected shopping cart solution, which wasn't my original intention - I like back end a little bit more - but this is my initial approach. 
  
  /C.
  
  ps
  
  Cred goes to these for providing the teaching on which this labApp(and some code) is based:
  
  Kudvenkat:  https://www.youtube.com/c/Csharp-video-tutorialsBlogspot
  
  Gavin Lon:  https://www.youtube.com/channel/UCa-Qgwt5VxN0iP3q6reHN6g
  
  Vladimir Pecanac and Marinko Spasojevic : https://code-maze.com
  
  "Ardalis": https://github.com/dotnet-architecture/eShopOnWeb
  
  ds
  
  
  
  
