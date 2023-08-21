using proyectoToken.Models.Custom;


namespace proyectoToken.Services
{
  public interface IAuthorizationService{
    Task<AuthorizationResponse> ReturnToken(AuthorizationRequest authorization); 
  }
}