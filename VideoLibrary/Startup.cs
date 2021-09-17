using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using VideoLibrary.DataContext;
using VideoLibrary.Models;
using VideoLibrary.Repository;
using VideoLibrary.Validators;

[assembly: FunctionsStartup(typeof(VideoLibrary.Startup))]
namespace VideoLibrary
{
    public class Startup : FunctionsStartup
    {
        public Startup()
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        }

        public IConfiguration Configuration { get; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IConfiguration>(Configuration);
            builder.Services.AddTransient<IValidator<Video>, SaveVideoValidator>();
            builder.Services.AddTransient<IValidator<Video>, LoadVideoValidator>();

            //Dapper config
            builder.Services.AddSingleton<DapperContext>();
            builder.Services.AddScoped<IVideoRepo, VideoRepo>();
        }
    }
}
