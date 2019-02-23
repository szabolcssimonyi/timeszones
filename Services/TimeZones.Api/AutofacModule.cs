using Autofac;
using TimeZones.Domain.Repositories;
using TimeZones.Domain.Seeders;
using TimeZones.Extensibility.Interfaces;
using TimeZones.Service;

namespace TimeZones.Api
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ApplicationSeeder>().As<IDomainSeeder>();
            builder.RegisterType<TimeZonesSeeder>().As<IDomainSeeder>();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<TimeZonesConverter>().As<IConverter>();
            builder.RegisterType<TimeZonesRepository>().As<ITimeZonesRepository>();
        }
    }
}
