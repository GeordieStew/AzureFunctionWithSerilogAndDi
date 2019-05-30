using AzureFunctionWithSerilogAndDi;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

[assembly: WebJobsStartup(typeof(Startup))]
namespace AzureFunctionWithSerilogAndDi
{
    public class Startup : IWebJobsStartup
    {
        IConfiguration Configuration;

        public Startup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .CreateLogger();
        }

        public void Configure(IWebJobsBuilder builder)
        {
            ConfigureServices(builder.Services).BuildServiceProvider(true);
        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var local_root = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azure_root = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";

            var actual_root = local_root ?? azure_root;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(actual_root)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var setting = Configuration["Setting"];
            //services.Configure<SqlOptions>(o => o.ConnectionString = Configuration["SqlConnectionString"]);

            services.AddLogging(logbuilder => logbuilder.AddSerilog(dispose: true));

            return services;
        }
    }
}
