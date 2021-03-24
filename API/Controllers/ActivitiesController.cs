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
        public async Task<ActionResult<List<Activity>>> GetActivities(CancellationToken ct)
        {
            //return await _context.Activities.ToListAsync();
            return await Mediator.Send(new List.Query(), ct); //we will use the Send method to instantiate a new instance of our handler
            //ct needs to be passed so the List handler knows when/if user has cancelled the request (by closing the browser, or something)
        }

        [HttpGet("{id}")] //activities/id
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            //return await _context.Activities.FindAsync(id); //we really would want to add some handling in case id doesn't exist....
            return await Mediator.Send(new Details.Query { Id = id }); //Details.Query takes an ID parameter
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            //because we're inheriting the [ApiController] attribute from our BaseApiController, our CreateActivity method knows it needs to look inside the body of the request to get the Activity object
            //otherwise, we'd need to put [FromBody]Activity activity as our parameter

            return Ok(await Mediator.Send(new Create.Command { Activity = activity }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return Ok(await Mediator.Send(new Edit.Command { Activity = activity }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}