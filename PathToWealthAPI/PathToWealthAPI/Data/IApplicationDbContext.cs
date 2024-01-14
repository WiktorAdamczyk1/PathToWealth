using Microsoft.EntityFrameworkCore;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User> User { get; set; }
        DbSet<RefreshToken> RefreshToken { get; set; }
        DbSet<UserFinancialData> UserFinancialData { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
