using FluentValidation;

namespace FluentOptionsValidation.SampleApp
{
    public class RedisSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class RedisSettingsValidator : AbstractValidator<RedisSettings>
    {
        public RedisSettingsValidator()
        {
            RuleFor(x => x.Host).Equal("localhost");
            RuleFor(x => x.Port).GreaterThan(0);
        }
    }
}