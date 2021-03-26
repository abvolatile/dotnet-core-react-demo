using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Activity
    {
        //each Activity is a record in the Db Table: Activities (over in Persistence > DataContext.cs)
        //BEFORE doing db migrations, make sure you stop your server
        //we *could* put [Required] attributes on all of these, but this isn't where we should be doing validation in a clean architecture project
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
    }
}