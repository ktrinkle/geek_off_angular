using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GeekOff.Services;

namespace GeekOff.Config
{
    public static class ServicesConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.TryAddScoped<IScoreService, ScoreService>();
        }
    }
}
