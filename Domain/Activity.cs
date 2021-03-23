using System;

namespace Domain
{
    public class Activity
    {
        //each Activity is a record in the Db Table: Activities (over in Persistence > DataContext.cs)
        //BEFORE doing db migrations, make sure you stop your server
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
    }
}