using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //we could set the below to a variable then throw an exception if we get a null return value, but 
                //Exceptions are "heavy" and we don't want to use them for control flow - instead we'll create Result objects
                //return await _context.Activities.FindAsync(request.Id);

                var activity = await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == request.Id); //can't use FindAsync when using the .ProjectTo autoMapper query method

                return Result<ActivityDto>.Success(activity);
                //we don't need to specify failure b/c it's either the activity or null, but we do need to test the result in our ActivitiesController
            }
        }
    }
}