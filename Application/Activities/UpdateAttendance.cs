using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Utilities;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities
                    .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
                    .FirstOrDefaultAsync(x => x.Id == request.Id); //you could use SingleOrDefaultAsync as well, but if more than one it'll error
                if (activity == null) return null;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                if (user == null) return null;

                var hostUsername = activity.Attendees.FirstOrDefault(h => h.IsHost)?.AppUser?.UserName; //optional chaining (temp)
                var attendance = activity.Attendees.FirstOrDefault(a => a.AppUser.UserName == user.UserName);

                //if user is host cancel the activity, otherwise remove them from it:
                if (attendance != null && hostUsername == user.UserName) activity.IsCancelled = !activity.IsCancelled;
                if (attendance != null && hostUsername != user.UserName) activity.Attendees.Remove(attendance);

                //if user isn't attending the Activity, then add them to it:
                if (attendance == null)
                {
                    attendance = new ActivityAttendee
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    activity.Attendees.Add(attendance);
                }

                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");
            }
        }
    }
}