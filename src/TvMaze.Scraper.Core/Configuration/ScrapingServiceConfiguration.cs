﻿namespace TvMaze.Scraper.Core.Configuration;

public sealed record ScrapingServiceConfiguration(TimeSpan TimeToWaitBeforeRetryTooManyRequests);