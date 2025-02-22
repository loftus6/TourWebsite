using Microsoft.AspNetCore.Authorization;

namespace TourWebsite.Areas.Identity
{
    public class AdminAccessPolicy : IAuthorizationRequirement
    {
    }

    public class AdminAccessPolicyHandler : AuthorizationHandler<AdminAccessPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminAccessPolicy requirement)
        {

            if (context.User.IsInRole("Admin")) //Admins and editors can access any page
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }


            context.Fail();
            return Task.CompletedTask;



        }
    }
}
