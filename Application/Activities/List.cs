using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    //a class that will List our Activities (nothing we didn't have before, but we've separated out our business logic)
        //we will modify our ActivitiesController
    public class List
    {
        //the way we use MediatR is to create a nested class (or classes) inside the List class (List.Query, List.Handler)
        public class Query : IRequest<List<Activity>> { } //simply returning a list here - no need for props

        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            //we pass in a query and get a list of activities in return - we need access to our DataContext in here
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            //this returns a list of activities, we have access to our query (and a cancellationToken - more later)
            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Activities.ToListAsync();
            }
        }
    }
}