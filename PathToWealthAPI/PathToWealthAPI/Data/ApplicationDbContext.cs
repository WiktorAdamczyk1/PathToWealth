using Microsoft.EntityFrameworkCore;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<JwtToken> JwtToken { get; set; }
        public DbSet<UserFinancialData> UserFinancialData { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
