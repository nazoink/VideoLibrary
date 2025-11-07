using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using VideoLibrary.DataContext;
using VideoLibrary.Models;
using VideoLibrary.Repository;
using VideoLibrary.Validators;

var host = Host.CreateDefaultBuilder(args)
 .ConfigureAppConfiguration((context, config) =>
 {
 config.SetBasePath(Directory.GetCurrentDirectory())
 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
 .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
 .AddEnvironmentVariables();
 })
 .ConfigureFunctionsWorkerDefaults()
 .ConfigureServices((context, services) =>
 {
 services.AddSingleton<IConfiguration>(context.Configuration);
 services.AddTransient<IValidator<Video>, SaveVideoValidator>();
 services.AddTransient<IValidator<Video>, LoadVideoValidator>();

 // Dapper config
 services.AddSingleton<DapperContext>();
 services.AddScoped<IVideoRepo, VideoRepo>();
 })
 .Build();

host.Run();
