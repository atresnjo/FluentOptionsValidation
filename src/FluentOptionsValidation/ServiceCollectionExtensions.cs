using System;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FluentOptionsValidation
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the startup validation filter with the section name
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="section">Section from the configuration file</param>
        /// <param name="configurationExpression"></param>
        public static IServiceCollection ConfigureFluentOptions<TOptions>(
            this IServiceCollection services,
            IConfiguration config, string section,
            Action<FluentOptionsConfiguration> configurationExpression = null)
            where TOptions : class, new()
        {
            return AddFluentOptions<TOptions>(services, config, section, configurationExpression);
        }

        /// <summary>
        /// Configures the startup validation filter by getting the section name automatically from the type
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="configurationExpression"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureFluentOptions<TOptions>(
            this IServiceCollection services,
            IConfiguration config,
            Action<FluentOptionsConfiguration> configurationExpression = null)
            where TOptions : class, new()
        {
            var section = typeof(TOptions).Name;
            return AddFluentOptions<TOptions>(services, config, section, configurationExpression);
        }

        private static IServiceCollection AddFluentOptions<TOptions>(IServiceCollection services,
            IConfiguration config, string section, Action<FluentOptionsConfiguration> configurationExpression = null)
            where TOptions : class, new()
        {
            var fluentOptionsConfiguration = new FluentOptionsConfiguration();
            configurationExpression?.Invoke(fluentOptionsConfiguration);
            services.Configure<TOptions>(config.GetSection(section));
            services.AddTransient<IStartupFilter, FluentOptionsStartupFilter<TOptions>>(provider =>
            {
                var validator = provider.GetService<IValidator<TOptions>>();
                var options = provider.GetService<IOptions<TOptions>>();
                var logger = provider.GetService<ILogger<TOptions>>();
                var validationFilter = new FluentOptionsStartupFilter<TOptions>(validator, options, logger)
                    {Configuration = fluentOptionsConfiguration};

                return validationFilter;
            });

            return services;
        }
    }
}