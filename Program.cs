var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// ============= SWAGGER CONFIGURATION =============
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "BODE-API",
        Version = "v1",
        Description = "Backend Of Diagnostic & Engine - Auto Repair Management System",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact {
            Name = "BODE Support",
            Email = "support@bodeapi.com",
            Url = new Uri("https://bodeapi.com")
        },
        License = new OpenApiLicense {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Add XML comments for better Swagger documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Add enum descriptions
    c.UseInlineDefinitionsForEnums();

    // Add bearer auth (if you add JWT later)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Group by controller
    c.TagActionsBy(api => new[] { api.ActionDescriptor.RouteValues["controller"] });
    c.DocInclusionPredicate((_, _) => true);
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Services
builder.Services.AddScoped<IServiceOrderService, ServiceOrderService>();
builder.Services.AddScoped<IDiagnosticService, DiagnosticService>();
builder.Services.AddScoped<IInspectionService, InspectionService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ============= SWAGGER MIDDLEWARE =============
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BODE-API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "BODE-API Documentation";
        c.DisplayRequestDuration();
        c.EnableTryItOutByDefault();
        c.ShowExtensions();
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Global exception handler
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

// Auto migrate database
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.GetPendingMigrations().Any()) {
        db.Database.Migrate();
    }
}

app.Run();