using api.Authorization.Model;
using api.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace api.Authorization
{
    public class ResourceOwnerAuthorizationHandler : AuthorizationHandler<ResourceOwnerRequirement, IUserOwnedResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOwnerRequirement requirement, IUserOwnedResource resource)
        {
            if (context.User.IsInRole(Roles.Admin) || context.User.FindFirstValue(ClaimTypes.NameIdentifier) == resource.UserId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public record ResourceOwnerRequirement : IAuthorizationRequirement;
} 
