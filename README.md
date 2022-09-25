# ShoppingDemo

This is a private Repo. 
Using Net.Core 3.1

Code is not for production. 
Just trying out some ideas...and will build on this foundation.

If You want to try out this code live - instead of downloading - go to:

To use standardUser priveligies, just register. 
To use admin priveligies, log in as TestUser1@hotmail.com, and use this password: "".

Feel free to try out funcionality, either as admin, standardUser or Manager, to try out auhtorization policies, or as unauthorized user.

If you want to download and test the code:

1. Download, extract, rebuild to check everything is downloaded.
2. Insert/change DB connectionstring in Appsettings.json and adjust Start.cs - on the wep api.
3. Set as multiple start up projects, API on top and Web below.
4. From Package Manager, make sure you run the Api - then to seed data, from the cmd line: Update-database  - it s h o u l d work.
5. Run ctrl F5.
