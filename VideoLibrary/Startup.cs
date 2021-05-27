using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;

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
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        public IConfigurationRoot Configuration { get; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            //builder.Services.AddSingleton<IMyService>((s) => {
            //    return new MyService();
            //});

            //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }

        public void ConfigurationServices(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();
            Debug.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            Debug.WriteLine(context);
            //Debug.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            Debug.WriteLine(Environment.GetEnvironmentVariable("ConnectionStrings-VideoLibrary-DB"));
            //builder.ConfigurationBuilder
            //    //.AddJsonFile(Path.Combine(context.ApplicationRootPath, "launchSettings.json"), optional: true, reloadOnChange: false)
            //    .AddJsonFile(Path.Combine(context.ApplicationRootPath, "local.settings.json"), optional: true, reloadOnChange: false)
            //    //.AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
            //    //.AddJsonFile(Path.Combine(context.ApplicationRootPath, $"{context.EnvironmentName}.appsettings.json"), optional: true, reloadOnChange: false)
            //    .AddEnvironmentVariables()
            //    .Build();
            //Debug.WriteLine();
            string value = Configuration.GetSection("ConnectionStrings-VideoLibrary-DB").Value;
        }
    }
}
