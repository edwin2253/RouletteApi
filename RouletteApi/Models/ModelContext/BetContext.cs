using Microsoft.EntityFrameworkCore;

namespace RouletteApi.Models.ModelContext
{
    public class BetContext : DbContext
    {
        public BetContext(DbContextOptions<BetContext> options) : base(options) { }
        public DbSet<Bet> Bets { get; set; }
    }
}
