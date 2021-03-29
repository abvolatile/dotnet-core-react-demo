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
    }
}