using System.Security.Claims;


namespace Shopping.Api
{
    public class CustomClaims
    {
        public static List<Claim> AllClaims = new List<Claim>()
    {
        new Claim("Create Role", "Create Role"),
        new Claim("Edit Role","Edit Role"),
        new Claim("Delete Role","Delete Role")
    };

    }
}
