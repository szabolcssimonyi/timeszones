namespace TimeZones.Extensibility
{
    public class Configuration
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
}
