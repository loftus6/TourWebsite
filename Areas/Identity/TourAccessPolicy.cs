using Microsoft.AspNetCore.Authorization;

namespace TourWebsite.Areas.Identity
{
    public class TourAccessPolicy : IAuthorizationRequirement
    {
    }

    public class TourAccessPolicyHandler : AuthorizationHandler<TourAccessPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TourAccessPolicy requirement)
        {
            string[] allowedUsers = context.Resource as string[];

            if (context.User.IsInRole("Admin") || context.User.IsInRole("Editor")) //Admins and editors can access any page
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //if (!context.User.IsInRole("User"))
            //{
            //    context.Fail();
            //    return Task.CompletedTask;
            //}

            if(allowedUsers== null)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (allowedUsers.Any(user => user.Equals(context.User.Identity.Name, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
