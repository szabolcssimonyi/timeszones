namespace TimeZones.Extensibility.Interfaces
{
    public interface IConverter
    {
        U Convert<T, U>(T model);
    }
}
