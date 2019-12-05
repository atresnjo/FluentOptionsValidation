using System;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FluentOptionsValidation
{
    public class FluentOptionsStartupFilter<T> : IStartupFilter where T : class, new()
    {
        private readonly IValidator<T> _validatableOptions;
        private readonly IOptions<T> _options;
        private readonly ILogger<T> _logger;
        public FluentOptionsConfiguration Configuration { get; set; }

        public FluentOptionsStartupFilter(IValidator<T> validatableOptions, IOptions<T> options, ILogger<T> logger)
        {
            _validatableOptions = validatableOptions;
            _options = options;
            _logger = logger;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            var result = _validatableOptions.Validate(_options.Value);
            if (result.IsValid)
                return next;

            var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
            var exceptionMessage = errors.Aggregate((error, foo) => $"{error} \n");

            if (Configuration.AbortStartupOnError)
                throw new AbortStartupException(exceptionMessage);

            foreach (var error in errors)
                _logger.LogError(error);

            return next;
        }
    }
}