using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeZones.Domain.Contexts;
using TimeZones.Extensibility.Dto;
using TimeZones.Extensibility.Entities;
using TimeZones.Extensibility.Interfaces;

namespace TimeZones.Domain.Repositories
{
    public class TimeZonesRepository : ITimeZonesRepository
    {
        private readonly IConverter converter;
        private readonly TimeZonesDbContext context;

        public TimeZonesRepository(TimeZonesDbContext context, IConverter converter)
        {
            this.converter = converter;
            this.context = context;
        }

        public async Task<IEnumerable<T>> GetAsync<T>()
        {
            var entities = await context.TimeZones.ToListAsync();
            return converter.Convert<IEnumerable<TimeZoneEntity>, IEnumerable<T>>(entities);
        }

        public async Task<IEnumerable<T>> GetAsync<T>(string userId)
        {
            var utzs = await context.UserTimeZones
                .Where(utz => utz.UserId == userId)
                .Select(utz => utz.TimeZoneEntity)
                .Distinct().ToListAsync();
            return converter.Convert<IEnumerable<TimeZoneEntity>, IEnumerable<T>>(utzs);
        }

        public async Task SaveUserTimeZoneAsync(UserTimeZoneDto dto)
        {
            if (context.UserTimeZones.Any(utz =>
                utz.UserId == dto.UserId && utz.TimeZoneEntityId == dto.TimeZoneEntityId))
            {
                return;
            }

            context.UserTimeZones.Add(converter.Convert<UserTimeZoneDto, UserTimeZoneEntity>(dto));
            await context.SaveChangesAsync();
        }
    }
}
