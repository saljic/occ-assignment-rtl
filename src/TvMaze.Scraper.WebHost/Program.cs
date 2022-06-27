using TvMaze.Scraper.App.Startup;
using TvMaze.Scraper.Core.Startup;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .ConfigureCore()
    .ConfigureApp();

builder
    .Build()
    .ConfigureApp()
    .Run();