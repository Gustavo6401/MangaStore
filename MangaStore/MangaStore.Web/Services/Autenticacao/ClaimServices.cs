using System.Security.Claims;

namespace MangaStore.Web.Services.Autenticacao
{
    public class ClaimServices
    {
        public string RetornarClaim(HttpContext context)
        {
            ClaimsPrincipal claims = context.User;

            foreach (Claim claim in claims.Claims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    return claim.Value;
                }
            }

            return null;
        }
    }
}
