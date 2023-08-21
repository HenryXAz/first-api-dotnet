
using Microsoft.AspNetCore.Mvc;
using proyectoToken.Models.Custom;
using proyectoToken.Services;

namespace proyectoToken.Controllers
{
  [Route("/api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IAuthorizationService _authorizationService;

    public UserController(IAuthorizationService authorizationService)
    {
      _authorizationService = authorizationService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> login([FromBody] AuthorizationRequest authorization)
    {
      var resultAuthorization = await this._authorizationService.ReturnToken(authorization);

      if(resultAuthorization == null) {
        return Unauthorized();
      }

      return Ok(resultAuthorization);
    }
  }
}