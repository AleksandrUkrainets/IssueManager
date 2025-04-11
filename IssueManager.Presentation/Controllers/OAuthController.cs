using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IssueManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController(IAuthService authService) : ControllerBase
    {
        [HttpGet("authUrl")]
        public IActionResult GetAuthUrl([FromQuery] string provider)
        {
            var authUrl = authService.GetAuthUrl(provider);
            if (authUrl == null) return BadRequest("Unsupported provider");

            return Ok(new { authUrl });
        }

        [HttpGet("signIn")]
        public async Task<IActionResult> SignIn([FromQuery] string code, [FromQuery] string state)
        {
            var tokenResult = await authService.SignIn(code, state);
            if (!tokenResult.IsSuccess) return BadRequest(tokenResult.Error);

            return Ok(new { token = tokenResult.Jwt });
        }
    }
}
