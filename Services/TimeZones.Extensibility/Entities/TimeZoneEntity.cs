namespace TimeZones.Extensibility.Entities
{
    public class TimeZoneEntity
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Offset { get; set; }

    }
}
