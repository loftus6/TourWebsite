## Capstone 490 Setup
Hello, this is my github for my capstone submission. The website is currently hosted at https://fairfieldhistory.com, but if that is down or you wish to create your own local version you can follow these steps.

#### Setup
If you are on windows, install visual studio and install the asp.net web development tools. I used .NET 8 as my version, which may be required for compatibility. 

Afterwards, open the sln with visual studio. If all goes well, the proper libraries should be includes and download automatically.

#### Database
You must have sql server installed on your computer. You can use the visual studio server explorer to see if you have any databases running locally.

Make sure you go into appsettings.json and change "TourWebsiteContextConnection" to your connection string for your local server, which may be similar to:
_
Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False_

Then open the package manager console and run

_Add-Migration InitalCreate_ (The second parameter can be any name)

Followed by

_Update-Database_

At this point it should be able to run by pressing the green play button.

If you need clarification or the hosted site is down, contact me at loftus6@marshall.edu


