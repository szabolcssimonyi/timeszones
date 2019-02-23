using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeZones.Domain.Contexts;
using TimeZones.Extensibility.Entities;
using TimeZones.Extensibility.Interfaces;

namespace TimeZones.Domain.Seeders
{
    public class TimeZonesSeeder : IDomainSeeder
    {
        private readonly TimeZonesDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly List<TimeZoneEntity> zones = new List<TimeZoneEntity>
        {
            new TimeZoneEntity
            {
                Abbreviation = "UTC",
                Location = "WorldWide",
                Name = "Coordinated Universal Time",
                Offset = 0
            },
            new TimeZoneEntity
            {
                Abbreviation = "CET",
                Location = "Europe",
                Name = "Central European Time",
                Offset = 1
            },
            new TimeZoneEntity
            {
                Abbreviation = "CST",
                Location = "Asia",
                Name = "China Standard Time",
                Offset = 4
            },
            new TimeZoneEntity
            {
                Abbreviation = "CDT",
                Location = "North America",
                Name = "Central Daylight Time",
                Offset = -5
            },
            new TimeZoneEntity
            {
                Abbreviation = "BST",
                Location = "Europa",
                Name = "British Summer Time",
                Offset = 1
            }
        };

        public TimeZonesSeeder(TimeZonesDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                var exceptions = await context.TimeZones.Select(z => z.Abbreviation).ToListAsync();
                var inserts = zones.Where(z => !exceptions.Contains(z.Abbreviation));
                await context.TimeZones.AddRangeAsync(inserts);
                await context.SaveChangesAsync();
                var admin = await userManager.FindByNameAsync("AdminUser");
                var def = await userManager.FindByNameAsync("DefaultUser");
                var utc = await context.TimeZones.SingleAsync(tz => string.Equals(tz.Abbreviation, "utc", StringComparison.OrdinalIgnoreCase));
                await SeedZoneForUser(admin, utc);
                await SeedZoneForUser(def, utc);
                await context.SaveChangesAsync();
                transaction.Commit();
            }
        }

        private async Task SeedZoneForUser(IdentityUser user, TimeZoneEntity zone)
        {
            if (!await context.UserTimeZones.AnyAsync(utz => utz.UserId == user.Id && utz.TimeZoneEntityId == zone.Id))
            {
                context.UserTimeZones.Add(new UserTimeZoneEntity
                {
                    TimeZoneEntityId = zone.Id,
                    UserId = user.Id
                });
            }
        }
    }
}
