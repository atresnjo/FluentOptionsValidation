using FluentOptionsValidation.SampleAppDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FluentOptionsValidation.SampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly IOptions<RedisSettings> _redisSettings;

        public SettingsController(IOptions<EmailSettings> emailSettings, IOptions<RedisSettings> redisSettings)
        {
            _emailSettings = emailSettings;
            _redisSettings = redisSettings;
        }

        [HttpPost]
        public IActionResult GetSettings()
        {
            return Ok($"{_emailSettings.Value.Email} - {_redisSettings.Value.Port}");
        }
    }
}