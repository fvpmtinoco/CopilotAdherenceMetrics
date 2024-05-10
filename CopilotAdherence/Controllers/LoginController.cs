using CopilotAdherence.Features.WeatherForecast;
using CopilotAdherence.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CopilotAdherence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new LoginCommand(request.Username, request.Password));

            if (result != null)
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(result) });

            return Unauthorized();
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, JwtSecurityToken?>
    {
        private readonly ILoginService _loginService;

        public LoginCommandHandler(ILoginService loginService)
        => _loginService = loginService;

        public async Task<JwtSecurityToken?> Handle(LoginCommand command, CancellationToken cancellationToken)
            => await _loginService.ValidCredentials(command.Username, command.Password);
    }


    public record LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }



    public class LoginCommand(string username, string password) : IRequest<JwtSecurityToken?>
    {
        public string Username => username;
        public string Password => password;
    }
}
