using Application.Utilities;
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


        //this method is so we don't have to clutter up our ActivitiesController with this a bunch of times
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();

            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            if (result.IsSuccess && result.Value == null) return NotFound();

            return BadRequest(result.Error);
        }
    }
}