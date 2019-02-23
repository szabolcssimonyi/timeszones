using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TimeZones.Extensibility;

namespace TimeZones.Domain.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly string connectionString;
        private readonly IOptions<Configuration> options;

        public ApplicationDbContext(IOptions<Configuration> options)
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
