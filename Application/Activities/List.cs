using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    //a class that will List our Activities (nothing we didn't have before, but we've separated out our business logic)
    //we will modify our ActivitiesController
    public class List
    {
        //the way we use MediatR is to create a nested class (or classes) inside the List class (List.Query, List.Handler)
        public class Query : IRequest<Result<List<ActivityDto>>> { } //simply returning a list here - no need for props

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            //we pass in a query and get a list of activities in return - we need access to our DataContext in here
            private readonly DataContext _context;
            private readonly ILogger<List> _logger;
            private readonly IMapper _mapper;

            public Handler(DataContext context, ILogger<List> logger, IMapper mapper)
            {
                _mapper = mapper;
                _logger = logger;
                _context = context;
            }

            //this returns a list of activities, we have access to our query (and a cancellationToken - more later)
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //the cancellationToken gives us the ability to cancel a long-running request (user initiated)
                //the try/catch is just for demonstration purposes - these requests are quick, so we need to fake a long one to cancel (in Postman)
                // try
                // {
                //     for (var i = 0; i < 10; i++)
                //     {
                //         cancellationToken.ThrowIfCancellationRequested();
                //         await Task.Delay(1000, cancellationToken);
                //         _logger.LogInformation($"Task {i} has completed");
                //     }
                // }
                // catch (System.Exception ex)
                // {
                //     _logger.LogInformation($"Task was cancelled: {ex}");
                // }

                // var activities = await _context.Activities
                //     .Include(a => a.Attendees)
                //     .ThenInclude(u => u.AppUser)
                //     .ToListAsync(cancellationToken); //this uses Eager loading (inefficient when we have user table with so many unused props)
                //we can't return the unmapped activities b/c .Include and .ThenInclude would grab an endless loop of Attendees and AppUsers...
                //var mappedActivities = _mapper.Map<List<ActivityDto>>(activities);

                //instead, we'll use an AutoMapper extension to only get the props we really want 
                //(we could also just hand-code the query using a .Select() method from linq):
                var activities = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                //because we're already projecting to the correct type (ActivityDto), we don't need to do the extra part where we're mapping it

                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}