using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //api/activities
    public class BaseApiController : ControllerBase
    {
        //now all of our controllers will have access to our Mediator
        private IMediator _mediator;

        //protected = available to any derived classes and the base controller itself
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        // the ??= is a null coalescing assignment operator (if _mediator is null, then use ...)

    }
}