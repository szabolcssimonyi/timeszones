using System.Collections.Generic;
using System.Threading.Tasks;
using TimeZones.Extensibility.Dto;

namespace TimeZones.Extensibility.Interfaces
{
    public interface ITimeZonesRepository
    {
        Task<IEnumerable<T>> GetAsync<T>();
        Task<IEnumerable<T>> GetAsync<T>(string userId);

        Task SaveUserTimeZoneAsync(UserTimeZoneDto dto);
    }
}
