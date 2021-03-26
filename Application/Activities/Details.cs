using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Utilities;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<Activity>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Activity>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                //we could set the below to a variable then throw an exception if we get a null return value, but 
                //Exceptions are "heavy" and we don't want to use them for control flow - instead we'll create Result objects
                //return await _context.Activities.FindAsync(request.Id);

                var activity = await _context.Activities.FindAsync(request.Id);

                return Result<Activity>.Success(activity);
                //we don't need to specify failure b/c it's either the activity or null, but we do need to test the result in our ActivitiesController
            }
        }
    }
}