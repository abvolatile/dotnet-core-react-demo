using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {

    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IsHostRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            //get user id - NameIdentifier (ID) b/c table key is UserId + ActivityId
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Task.CompletedTask; //meaning user is not authorized

            var activityId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues
                .SingleOrDefault(a => a.Key == "id").Value?.ToString()); //parsing the activity Id from the read parameters

            var attendee = _dbContext.ActivityAttendees
                .AsNoTracking() //we need this here to make sure we don't keep this attendee object in memory (otherwise we will lose it)
                .SingleOrDefaultAsync(aa => aa.AppUserId == userId && aa.ActivityId == activityId)
                .Result; //can't use FindAsync(userId, activityId) w/ .AsNoTracking(), btw

            if (attendee == null) return Task.CompletedTask;

            if (attendee.IsHost) context.Succeed(requirement);

            return Task.CompletedTask; // if attendee is present but not host
        }
    }
}