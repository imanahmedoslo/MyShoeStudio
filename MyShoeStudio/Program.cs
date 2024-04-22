using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyShoeStudio.Data;
using MyShoeStudio.Data.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using MyShoeStudio.MiddleWear;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuration for JWT
var jwtIssuer = Environment.GetEnvironmentVariable("_JWT_ISSUER");
var jwtKey = Environment.GetEnvironmentVariable("_JWT_KEY_");

var corsSettings = new CorsSettings();
if (builder.Environment.IsProduction())
{
    corsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>();
}


// Add services to the container.
//var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:_DEFAULTCONNECTION_");
if (builder.Environment.IsDevelopment())
{
    var connectionString = Environment.GetEnvironmentVariable("_DEFAULTCONNECTION_");
    // Use SQL Server for Development
    builder.Services.AddDbContext<MyShoeStudioDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else if (builder.Environment.IsProduction())
{
    var connectionString = Environment.GetEnvironmentVariable("_DEFAULTCONNECTION_");
    // Use PostgreSQL for Production
    builder.Services.AddDbContext<MyShoeStudioDbContext>(options =>
        options.UseNpgsql(connectionString));
}

// Adding Identity
builder.Services.AddIdentity<User, IdentityRole>(options => {
    // Identity options configuration
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<MyShoeStudioDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Adding CORS
builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("DevCorsPolicy", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    }
    else if (builder.Environment.IsProduction())
    {
        options.AddPolicy("ProdCorsPolicy", policyBuilder =>
        {
            policyBuilder.WithOrigins(corsSettings?.AllowedOrigins.ToArray() ?? new string[] { "https://my-style-studio.vercel.app/" })
                         .AllowAnyMethod()
                         .AllowAnyHeader();
        });
    }
});


// Adding controllers and JSON options
builder.Services.AddControllers().AddJsonOptions(c =>
{
    c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Define the API Key scheme that Swagger UI will use
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. X-API-Key: My_API_Key",
        In = ParameterLocation.Header,
        Name = "_X_API_KEY_", // The name of the header to be used
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    Scheme = "ApiKey",
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
});

var app = builder.Build();
var key1 = app.Configuration.GetValue<String>("KEY");
  

// Middleware configuration

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
    app.UseDeveloperExceptionPage();
    app.UseCors("DevCorsPolicy");
}
else if(app.Environment.IsProduction())
{
    app.UseMiddleware<ApiKeyMiddleware>();
    app.UseCors("ProdCorsPolicy");
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseAuthentication(); // Make sure to call this before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }