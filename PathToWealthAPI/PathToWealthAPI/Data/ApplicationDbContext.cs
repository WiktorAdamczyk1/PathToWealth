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
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<UserFinancialData> UserFinancialData { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure cascade delete for User-RefreshToken relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure cascade delete for User-UserFinancialData relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserFinancialData)
                .WithOne(ufd => ufd.User)
                .HasForeignKey<UserFinancialData>(ufd => ufd.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
