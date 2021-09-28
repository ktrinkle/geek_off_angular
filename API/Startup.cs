using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using GeekOff.Config;
using GeekOff.Data;
using GeekOff.Services;

namespace GeekOff
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILogger logger)
        {
            Configuration = configuration;
            Environment = environment;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public ILogger Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                        builder => builder.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                );
            });

            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Computer Reset API", Version = "v1" });
                //future c.TagActionsBy();
        
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
        
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference 
                                { 
                                    Type = ReferenceType.SecurityScheme, 
                                    Id = "Bearer" 
                                }
                            },
                            new string[] {}
        
                    }
                });
            });
        

#endregion

            string postgresConn = Configuration["ConnectionStrings:GeekOff"];

            services.AddDbContext<contextGo>(options => options.UseNpgsql(postgresConn));

            services.AddCustomServices(Configuration);

            services.AddSignalR();
            
            // services.AddMicrosoftIdentityWebApiAuthentication(Configuration, "AzureAd");

            services.AddControllers();

            services.AddHttpClient();

            services.AddLogging();

            // services.AddApplicationInsightsTelemetry();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Computer Reset API");
                });

                app.UseDeveloperExceptionPage();

                app.UseCors(MyAllowSpecificOrigins);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<EventHub>("/events");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseCors(builder => builder.AllowAnyMethod().AllowCredentials());

        }
    }
}
