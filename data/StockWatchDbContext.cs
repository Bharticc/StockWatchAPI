using Microsoft.EntityFrameworkCore;
using StockWatchAPI.Models;

namespace StockWatchAPI.data
{
    public class StockWatchDbContext: DbContext
    {
        public StockWatchDbContext(DbContextOptions<StockWatchDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}
