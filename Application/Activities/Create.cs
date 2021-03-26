using System.Threading;
using System.Threading.Tasks;
using Application.Utilities;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        //Commands do not return anything
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; } //this is what we will receive as a parameter from our API
        }

        //sets a rule for validation using our ActivityValidator class (and FluentValidation)
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity); //right here we're just adding the activity in memory (not into the db yet - so we don't need to use the AddAsync)
                var result = await _context.SaveChangesAsync() > 0;
                //this returns an int representing the number of state changes to the db - so if it's 0, it failed to update

                if (!result) return Result<Unit>.Failure("Failed to create activity");

                //even though Commands don't return anything, we have to return Unit.Value because the Handle method expects a return value (MediatR.Unit - which is basically void)
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}