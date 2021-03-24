using System;
using System.Collections.Generic;
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
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            //return await _context.Activities.ToListAsync();
            return await Mediator.Send(new List.Query()); //we will use the Send method to instantiate a new instance of our handler
        }

        [HttpGet("{id}")] //activities/id
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            //return await _context.Activities.FindAsync(id); //we really would want to add some handling in case id doesn't exist....
            return await Mediator.Send(new Details.Query { Id = id }); //Details.Query takes an ID parameter
        }
    }
}