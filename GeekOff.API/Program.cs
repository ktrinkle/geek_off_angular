using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((_, _, config) =>
{
    if (builder.Environment.IsDevelopment())
    {
        config.MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console();
    }
    else
    {
        config.MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console();
    }
});

builder.Logging.AddApplicationInsights();

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings")
);

var postgresConn = builder.Configuration.GetConnectionString("GeekOff") ?? throw new InvalidOperationException("Connection string 'GeekOff' not found.");

builder.Services.AddDbContext<ContextGo>(options =>
    options.UseNpgsql(postgresConn));

builder.Services.AddControllersWithViews();

builder.Services.AddCustomServices();

builder.Services.AddSignalR();

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDev",
        builder => builder.WithOrigins("http://localhost:4200", "http://localhost:5000", "https://localhost:5001")
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .AllowAnyMethod());

    options.AddPolicy("Hosted",
        builder => builder.WithOrigins("https://geekoff.azurewebsites.net")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(3600)));
});

builder.Services.AddHealthChecks();

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.IncludeErrorDetails = true; // temporary
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        ValidateIssuerSigningKey = true,
        RequireSignedTokens = true,
        RequireExpirationTime = true, // <- JWTs are required to have "exp" property set
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"]!)) {KeyId = builder.Configuration["AppSettings:JWTKeyId"]}
    };
});

// handle enum conversion. We also could just allow text values from the app?

builder.Services.Configure<JsonOptions>(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

#region swagger

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c => {
        c.EnableAnnotations();
        c.CustomSchemaIds(s => s.FullName!.Replace("+", "."));

        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Geek Off API", Version = "v2" });

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
}


#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();

    app.UseCors("LocalDev");

}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();

    app.UseCors("Hosted");
}

app.UseRouting();

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<EventHub>("/events");

app.MapHealthChecks("/status");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
