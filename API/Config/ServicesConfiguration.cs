using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GeekOff.Services;
using GeekOff.Helpers;

namespace GeekOff.Config
{
    public static class ServicesConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.TryAddScoped<IManageEventService, ManageEventService>();
            services.TryAddScoped<IScoreService, ScoreService>();

            services.TryAddScoped<IClaimsTransformation, AddRolesClaimsTransformation>();
        }
    }
}
