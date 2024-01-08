using Microsoft.EntityFrameworkCore;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<JwtToken> JwtTokens { get; set; }
        public DbSet<UserFinancialData> UserFinancialData { get; set; }

    }
}
