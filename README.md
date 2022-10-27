# ShoppingDemo

A)

This is a private Repo. 

My Testsite - trying out different approaches and techniques!

Blazor Web Assembly upfront. 

Updated to .Net 6

Code is not for production. 

Just lab.

Next step 1: enhance shopping cart and bind to applicationUser - merge cart from Db and cart from LocalStorage.

Next step 2: Strong believer in not moving forward too fast. Need time to digest certain techniques, pros and cons
             when it comes to design and implementation choices. But as far as API concerns, I am going to try
             make my generic repository asynchronous all way threw, and use this new async, generic approach in a 
             more stream-lined fashion, aka following dry, reduce code duplication. Then, when that is done, t h e n I will look
             into aggregate technique, seperating domain logic use cases from search, where you typically don't
             need to interact with domain entities the same way(you just want query and summarize business results).

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
