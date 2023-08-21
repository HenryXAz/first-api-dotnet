
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace proyectoToken.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class CountryController : ControllerBase
  {

    [Authorize]
    [HttpGet()]
    [Route("countries")]
    public async Task<IActionResult> findAll()
    {
      var countryList = await Task.FromResult(new List<String>{
        "Francia",
        "Inglaterra",
        "Alemania",
        "Mexico",
        "China"
      });

      return Ok(countryList);
    }

  }
}