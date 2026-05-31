using System.Text;
using Authentication.Application;
using Authentication.Endpoints.Controllers;
using Authentication.Infrastructure;
using Authentication.Infrastructure.Database;
using Authentication.Infrastructure.Database.Seeds;
using Authentication.Infrastructure.Implementations.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Controllers — discover from all modules
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AuthController).Assembly);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token. Example: eyJhbGci..."
    });

    options.AddSecurityRequirement((document) => new()
    {
        [new("Bearer", document)] = []
    });

});

// Application & Infrastructure Services
builder.Services.AddAuthApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

// Database
builder.Services.AddDbContext<MainDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnectionString"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)));

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
if (jwtSettings != null)
{
    var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            // Map the role claim type correctly
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
        
        // Disable inbound claim type mapping to preserve custom claims
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        handler.InboundClaimTypeMap.Clear();
    });
}

var app = builder.Build();

// Initialize database with seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
    
    // Create database if it doesn't exist
    dbContext.Database.EnsureCreated();
    
    // Apply seed data only if database is empty
    if (!dbContext.Roles.Any())
    {
        var seedSql = InitialSeed.Up();
        dbContext.Database.ExecuteSqlRaw(seedSql);
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();