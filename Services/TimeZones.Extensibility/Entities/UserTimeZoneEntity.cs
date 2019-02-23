using System.Collections.Generic;

namespace TimeZones.Extensibility.Entities
{
    public class UserTimeZoneEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public int TimeZoneEntityId { get; set; }

        public TimeZoneEntity TimeZoneEntity { get; set; }

    }
}
