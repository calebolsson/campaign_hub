using campaign_hub.Models;
using Microsoft.EntityFrameworkCore;

namespace campaign_hub.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Campaign> Campaigns { get; set; }
    }
}
