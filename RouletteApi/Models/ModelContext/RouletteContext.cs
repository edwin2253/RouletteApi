using Microsoft.EntityFrameworkCore;

namespace RouletteApi.Models.ModelContext
{
    public class RouletteContext : DbContext
    {
        public RouletteContext(DbContextOptions<RouletteContext> options) : base(options) { }
        public DbSet<Roulette> Roulettes { get; set; }
    }
}
