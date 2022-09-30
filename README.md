# ShoppingDemo

A)

This is a private Repo. 

Blazor Web Assembly upfront. 

Using Net.Core 3.1

Code is not for production. 

Just lab.

Next step: enhance shopping cart and bind to applicationUser.

B)

If you want to try out this code live - instead of downloading - go to: NotImplementedYet!

StandardUser privileges - register with a FAKE email, such as FakeUser1@hotmail.com.

Admin privileges - log in as TestUser1@hotmail.com - password: "".

Feel free to try out authorization policies funcionality, either as admin, standardUser or Manager.

C)

If you want to download and test the code:

1. Download, extract, rebuild to check everything is downloaded.
2. Insert/change DB connectionstring in Appsettings.json and adjust Start.cs - on the wep api.
3. Set as multiple start-up projects from solution explorer, API on top and Web below.
4. From Package Manager, make sure you run the Api - then to seed data once, from the cmd line: Update-database  - it s h o u l d work.
5. Ctrl F5.
6. Note: if you register as a new user in Development, you will be given "Administrator" role by default. Change this to "standardUser" in AccountController/Registration,
   or something else(see db).
  
  /C.
