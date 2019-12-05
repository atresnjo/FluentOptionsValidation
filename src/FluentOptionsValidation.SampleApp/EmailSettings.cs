using FluentValidation;

namespace FluentOptionsValidation.SampleApp
{
    public class EmailSettings
    {
        public string Email { get; set; }
        public string Message { get; set; }
    }

    public class EmailSettingsValidator : AbstractValidator<EmailSettings>
    {
        public EmailSettingsValidator()
        {
            RuleFor(x => x.Email).EmailAddress().Equal("local@service.com");
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}