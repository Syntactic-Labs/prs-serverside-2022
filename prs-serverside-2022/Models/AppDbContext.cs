using Microsoft.EntityFrameworkCore;

namespace prs_serverside_2022.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder) { }
        public DbSet<User> ?Users { get; set; }
        public DbSet<Vendor> ?Vendor { get; set; }
    }
}
