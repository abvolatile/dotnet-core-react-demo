using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    //we don't need to specify a new DbSet for AppUser if we use the IdentityDbContext<AppUser> instead of just DbContext (like we had before)
    public class DataContext : IdentityDbContext<AppUser>
    {
        //basically our DbContext (DataContext) is an abstraction away from our Db - should be added in API > Startup > ConfigureServices
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; } //reflects the name of our Db table for the domain entity Activity
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        { //overriding the OnModelCreating method so we can use our own custom model for ActivityAttendees (because we wanted extra props)
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new { aa.AppUserId, aa.ActivityId })); //tells EF what to use for primary key in ActivityAttendees DbSet

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId); //tells model builder that each AppUser can have many Activities

            builder.Entity<ActivityAttendee>()
                .HasOne(a => a.Activity)
                .WithMany(u => u.Attendees)
                .HasForeignKey(aa => aa.ActivityId); //tells model builder that each Activity can have many Attendees (AppUsers)
        }
    }
}