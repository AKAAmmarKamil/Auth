using Microsoft.EntityFrameworkCore;
namespace Auth
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }

    }
}
