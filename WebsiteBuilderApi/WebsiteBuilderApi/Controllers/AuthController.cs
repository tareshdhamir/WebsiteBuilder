using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebsiteBuilderApi.Services;

namespace WebsiteBuilderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync();
            if (!authenticateResult.Succeeded)
                return Unauthorized();

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var providerId = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _authService.AuthenticateGoogleUserAsync(email, providerId);
            var token = _authService.GenerateJwtToken(user);

            // Redirect to frontend with token
            return Redirect($"http://localhost:4200/login-success?token={token}");
        }
    }
}
