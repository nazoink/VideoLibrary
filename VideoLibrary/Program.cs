using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using VideoLibrary.DataContext;
using VideoLibrary.Models;
using VideoLibrary.Repository;
using VideoLibrary.Validators;

// Program configures the Functions Worker host and registers application services.
var host = Host.CreateDefaultBuilder(args)
 .ConfigureAppConfiguration((context, config) =>
 {
 // Load configuration from appsettings files and environment.
 config.SetBasePath(Directory.GetCurrentDirectory())
 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
 .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
 .AddEnvironmentVariables();
 })
 .ConfigureFunctionsWorkerDefaults()
 .ConfigureServices((context, services) =>
 {
 // Make IConfiguration available via DI
 services.AddSingleton<IConfiguration>(context.Configuration);

 // Register validators. If you need multiple validators for the same model,
 // consider registering the concrete types and injecting IEnumerable<IValidator<T>>.
 services.AddTransient<IValidator<Video>, SaveVideoValidator>();
 services.AddTransient<IValidator<Video>, LoadVideoValidator>();

 // Dapper context and repository registrations
 services.AddSingleton<DapperContext>();
 services.AddScoped<IVideoRepo, VideoRepo>();
 })
 .Build();

host.Run();
