using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        //moving our context to a mediator (in Application > Activities > List), which we will add to our BaseApiController
        // private readonly DataContext _context;
        // public ActivitiesController(DataContext context)
        // {
        //     _context = context;
        // }

        [HttpGet]
        public async Task<IActionResult> GetActivities(CancellationToken ct)
        {
            //return await _context.Activities.ToListAsync(); //old

            return HandleResult(await Mediator.Send(new List.Query(), ct)); //we will use the Send method to instantiate a new instance of our handler
            //ct needs to be passed so the List handler knows when/if user has cancelled the request (by closing the browser, or something)
        }

        [HttpGet("{id}")] //activities/id
        public async Task<IActionResult> GetActivity(Guid id)
        {
            //return await _context.Activities.FindAsync(id); //way to do it without Mediator

            //in case id doesn't exist, if we don't have error handling, null will be returned (which is a 204 response - which is still "OK")
            //normal way to do that would be to assign the below to a variable then if it's null return NotFound() otherwise return the activity
            //however, we want to keep error handling out of our API controllers, so we're going to do it in our Application classes instead (business logic section)
            //**before, we were returning a Task<ActionResult<Action>>, but now we just need to return a Task<IActionResult>
            return HandleResult(await Mediator.Send(new Details.Query { Id = id })); //Details.Query takes an ID parameter

            //this HandleResult method is a function we created for our base controller so we can reuse the code
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            //because we're inheriting the [ApiController] attribute from our BaseApiController, our CreateActivity method knows it needs to look inside the body of the request to get the Activity object
            //otherwise, we'd need to put [FromBody]Activity activity as our parameter

            return HandleResult(await Mediator.Send(new Create.Command { Activity = activity }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}