﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Shopping.Api.Security
{
    public class AdminHandler : AuthorizationHandler<AdminRequirement>
    {


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                        AdminRequirement adminRequirement)
        {
            var authFilterContext = context.Resource as AuthorizationFilterContext;
            if (authFilterContext == null)
            {
                return Task.CompletedTask;
            }

            string loggedInAdminId =
                   context.User.Claims.FirstOrDefault
                   (c => c.Type == ClaimTypes.NameIdentifier).Value;

            string adminIdBeingEdited =
                   authFilterContext.HttpContext.Request.Query["userId"];

            if (context.User.IsInRole("Administrators") &&
                context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") &&
                adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower())
            {
                context.Succeed(adminRequirement);
            }
            return Task.CompletedTask;
        }
    }
}