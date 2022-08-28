# tretton37-web-scraper

### Description

This is a C# console application, created with .net 6.0 framework, which traverse recursively and downloads the content of (https://tretton37.com).

Project Solution contains 3 different c# project, which are the main console application, a class library for abstractions and unit test project with xUnit framework.
The program, first, retrieves the main page content and searches for all the links through the content. And decides if the link is for a file or route to another page. 
If it's a file, the file is saved with the name in the link; however if it's a route, the html content is being saved to it's folder with name `index.html`. 

### Requirements

[.Net 6.0 SDK](https://dotnet.microsoft.com/en-us/download)

### How to Run

1. Clone the repository to a desired folder.
2. Then open a terminal and navigate to the root folder of the repository.
3. Run the following command  to run the project:


`dotnet run --project Tretton37WebScraper/Tretton37WebScraper.csproj`

Alternatively, the solution can be opened with an IDE which supports .net and run directly via the IDE such as Visual Studio or JetBrains Rider.

When the program run is completed, all the html content and files is saved under folder `tretton37` where the command is run.

### How to Run Tests

It's assumed that the the repository is cloned already.

1. Open up a terminal and run the following command in the root folder:


`dotnet test Tretton37WebScraper.Tests/Tretton37WebScraper.Tests.csproj `