using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext
    {
        //basically our DbContext (DataContext) is an abstraction away from our Db - should be added in API > Startup > ConfigureServices
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; } //reflects the name of our Db table for the domain entity Activity
    }
}