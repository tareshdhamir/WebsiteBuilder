using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebsiteBuilderApi.DTOs;
using WebsiteBuilderApi.Services;

namespace WebsiteBuilderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TemplateController : ControllerBase
    {
        private readonly TemplateService _templateService;
        private readonly AzureDeploymentService _deploymentService;

        public TemplateController(TemplateService templateService, AzureDeploymentService deploymentService)
        {
            _templateService = templateService;
            _deploymentService = deploymentService;
        }

        [HttpGet]
        public IActionResult GetTemplates()
        {
            var templates = _templateService.GetTemplates();
            return Ok(templates);
        }

        [HttpPost("deploy")]
        public async Task<IActionResult> DeployTemplate([FromBody] DeploymentRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var containerName = $"user-{userId}-{Guid.NewGuid()}";
            var deployedUrl = await _deploymentService.DeployTemplateAsync(request.TemplatePath, containerName);
            return Ok(new { url = deployedUrl });
        }
    }
}
