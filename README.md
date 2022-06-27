# Introduction 
The TvMaze.Scraper application will scrape the https://api.tvmaze.com/ and store the data which will be exposed on /api/shows url. The data needs to be accessed through pagination e.g. /api/shows?pageIndex=0&itemCount=2 will return the first 2 shows.

# Getting Started
In order to run the application you can open up a powershell terminal and navigate to the TvMaze.Scraper.WebHost folder and call "dotnet run". The api should not be accessible on https://localhost:5001/