using Microsoft.AspNetCore.Identity;

namespace GeekOff
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(
            //    CertificateAuthenticationDefaults.AuthenticationScheme)
            //    .AddCertificate();

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(options =>{
            //         var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Secret"]));

            //         //new SigningCredentials(appSecurityKey, SecurityAlgorithms.HmacSha512Signature

            //         options.IncludeErrorDetails = true; // <- great for debugging

            //         // Configure the actual Bearer validation
            //         options.TokenValidationParameters = new TokenValidationParameters {
            //             IssuerSigningKey = symmetricKey,
            //             ValidAudience = Configuration["AppSettings:Audience"],
            //             ValidIssuer = Configuration["AppSettings:Issuer"],
            //             RequireSignedTokens = true,
            //             RequireExpirationTime = true, // <- JWTs are required to have "exp" property set
            //             ValidateLifetime = true, // <- the "exp" will be validated
            //             ValidateAudience = true,
            //             ValidateIssuer = true,
            //         };
            //     });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddAuthorization();
            services.AddControllers();

            #region swagger
            services.AddSwaggerGen(c => {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Geek Off API", Version = "v1.1" });
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
                            System.Array.Empty<string>()

                    }
                });
            });


            #endregion

            var postgresConn = Configuration["ConnectionStrings:GeekOff"];

            services.AddDbContext<ContextGo>(options => options.UseNpgsql(postgresConn));

            services.AddCustomServices();

            services.AddSignalR();

            services.AddControllers();

            services.AddHttpClient();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Player", policy =>
                    policy.RequireAssertion(context => context.User.IsInRole("player")));
                options.AddPolicy("Admin", policy =>
                    policy.RequireAssertion(context => context.User.IsInRole("admin")));
                options.AddPolicy("GeekOMatic", policy =>
                    policy.RequireAssertion(context => context.User.IsInRole("geekomatic")));
            });

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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Geek Off API"));

                app.UseDeveloperExceptionPage();

                app.UseCors(builder => builder.WithOrigins("http://localhost:4200")
                                                .AllowAnyMethod()
                                                .AllowCredentials()
                                                .AllowAnyHeader());

                app.UseCors(builder => builder.WithOrigins("http://localhost:5000")
                                    .AllowAnyMethod()
                                    .AllowCredentials()
                                    .AllowAnyHeader());
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapHub<EventHub>("/events");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseCors(builder => builder.WithOrigins("https://geekoff.azurewebites.net")
                                .AllowCredentials()
                                .AllowAnyMethod()
                                .AllowAnyHeader());

        }
    }
}
