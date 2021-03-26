using System.Threading;
using System.Threading.Tasks;
using Application.Utilities;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
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
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                //we need to get our Activity from the database and then update each field from the activity passed in
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                if (activity == null) return null;

                //activity.Title = request.Activity.Title ?? activity.Title; //Title change included in the request OR current if not included
                //could do it like the above, OR use a tool called AutoMapper (which we get from a NuGet Package)
                _mapper.Map(request.Activity, activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}