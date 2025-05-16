using Coin_Exchange.Models.Modal;
using Microsoft.EntityFrameworkCore;


namespace Coin_Exchange.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        { }

        public DbSet<User> Users { get; set; }

    }
}
