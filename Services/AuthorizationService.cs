
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using proyectoToken.Models;
using proyectoToken.Models.Custom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace proyectoToken.Services
{
  public class AuthorizationService : IAuthorizationService
  {

    private readonly ApiDotnetContext _context;
    private readonly IConfiguration _configuration;

    public AuthorizationService(ApiDotnetContext context, IConfiguration configuration)
    {

      _context = context;
      _configuration = configuration;
    }

    private String GenerateToken(String userId) {
      var key = _configuration.GetValue<String>("JwtSettings:key");
      var keyBytes = Encoding.ASCII.GetBytes(key);

      var claims = new ClaimsIdentity();
      claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));

      var tokenCredentials = new SigningCredentials(
        new SymmetricSecurityKey(keyBytes),
        SecurityAlgorithms.HmacSha256Signature
      );

      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = claims,
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials = tokenCredentials
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var tokenConfig = tokenHandler.CreateToken(tokenDescriptor); 

      string tokenCreated = tokenHandler.WriteToken(tokenConfig);

      return tokenCreated;
    }


    public async Task<AuthorizationResponse> ReturnToken(AuthorizationRequest authorization)
    {
      var userFounded = _context.Users.FirstOrDefault(user => 
        user.Name == authorization.UserName &&
        user.KeyUser == authorization.UserKey
      );

      if(userFounded == null) {
        return await Task.FromResult<AuthorizationResponse>(null);
      }

      String tokenCreated = this.GenerateToken(userFounded.ToString());
      
      return new AuthorizationResponse() {
        Token = tokenCreated,
        Result = true,
        Message = "OK"
      };
    }
  }




}