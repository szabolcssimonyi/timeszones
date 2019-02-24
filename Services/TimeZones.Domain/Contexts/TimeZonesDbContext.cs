using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TimeZones.Extensibility;
using TimeZones.Extensibility.Entities;

namespace TimeZones.Domain.Contexts
{
    public class TimeZonesDbContext : DbContext
    {
        private readonly string connectionString;
        private readonly IOptions<Configuration> options;
        public DbSet<TimeZoneEntity> TimeZones { get; set; }
        public DbSet<UserTimeZoneEntity> UserTimeZones { get; set; }

        public TimeZonesDbContext(IOptions<Configuration> options)
        {
            this.options = options;
            this.connectionString = options.Value.ConnectionStrings.DefaultConnection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("TimeZones.Api"));
        }

    }
}
