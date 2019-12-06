using FluentOptionsValidation.SampleAppDomain;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FluentOptionsValidation.SampleWorkerApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining(typeof(EmailSettings));
            services.ConfigureFluentOptions<EmailSettings>(Configuration,
                configuration => configuration.AbortStartupOnError = false);
            services.ConfigureFluentOptions<RedisSettings>(Configuration,
                configuration => configuration.AbortStartupOnError = true);
        }

        // Must be declared
        public void Configure()
        {

        }
    }

    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureServices((hostContext, services) => { services.AddHostedService<Worker>(); });
    }
}