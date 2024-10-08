using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GeekOff.Config;

public static class ServicesConfiguration
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        // Services
        services.TryAddScoped<ILoginService, LoginService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // services.TryAddScoped<IClaimsTransformation, AddRolesClaimsTransformation>();
    }
}

