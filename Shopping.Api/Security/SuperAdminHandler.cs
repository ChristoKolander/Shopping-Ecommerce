using Microsoft.AspNetCore.Authorization;

namespace Shopping.Api.Security
{
    public class SuperAdminHandler: AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminRequirement requirement)
        {
            if (context.User.IsInRole("SuperAdmins"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
    

    
}
