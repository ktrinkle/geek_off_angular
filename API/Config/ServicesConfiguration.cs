using System;
using GeekOff.Helpers;
using GeekOff.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GeekOff.Config
{
    public static class ServicesConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            // Services
            services.TryAddScoped<ILoginService, LoginService>();
            services.TryAddScoped<IManageEventService, ManageEventService>();
            services.TryAddScoped<IScoreService, ScoreService>();
            services.TryAddScoped<IQuestionService, QuestionService>();
            services.TryAddScoped<ITeamService, TeamService>();

           // services.TryAddScoped<IClaimsTransformation, AddRolesClaimsTransformation>();
        }
    }
}
