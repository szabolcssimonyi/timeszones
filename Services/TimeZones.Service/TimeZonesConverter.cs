using AutoMapper;
using TimeZones.Extensibility.Dto;
using TimeZones.Extensibility.Entities;
using TimeZones.Extensibility.Interfaces;

namespace TimeZones.Service
{
    public class TimeZonesConverter : IConverter
    {
        private readonly IMapper mapper;
        public TimeZonesConverter()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TimeZoneEntity, TimeZoneDto>();
                cfg.CreateMap<UserTimeZoneDto, UserTimeZoneEntity>();
            });
            mapper = config.CreateMapper();
        }

        public U Convert<T, U>(T model)
        {
            return mapper.Map<U>(model);
        }
    }
}
