using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        //Commands do not return anything
        public class Command : IRequest
        {
            public Activity Activity { get; set; } //this is what we will receive as a parameter from our API
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity); //right here we're just adding the activity in memory (not into the db yet - so we don't need to use the AddAsync)
                await _context.SaveChangesAsync();

                //even though Commands don't return anything, we have to add this because the Handle method expects a return value (MediatR.Unit - which is basically void)
                return Unit.Value;
            }
        }
    }
}