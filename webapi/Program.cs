using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Any;
using System.Security.Claims;
using dotenv.net;
using webapi.Repositories;
using webapi.Services;
using webapi.DataContexts;
using webapi.Models;
using webapi.Interfaces;
using webapi.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Last .env og miljøvariabler
DotEnv.Load();
builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Hent connection string basert på miljø
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string 'DefaultConnection' is not set.");
}

builder.Services.AddDbContext<NeighborhoodContext>(options =>
    options.UseNpgsql(connectionString));

// Auth-konfig
string jwtIssuer = builder.Configuration["AUTH_DOMAIN"] ?? throw new ArgumentNullException("AUTH_DOMAIN");
string jwtAudience = builder.Configuration["AUTH_AUDIENCE"] ?? throw new ArgumentNullException("AUTH_AUDIENCE");

HttpClient client = new();
string googleKeys = client.GetStringAsync("https://www.googleapis.com/robot/v1/metadata/x509/securetoken@system.gserviceaccount.com").Result;
IEnumerable<SecurityKey> jwtKeyset = new JsonWebKeySet(googleKeys).GetSigningKeys();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<NeighborhoodContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true;
    options.Authority = jwtIssuer;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ValidAlgorithms = [SecurityAlgorithms.RsaSha256],
        ValidateIssuerSigningKey = true,
        IssuerSigningKeys = jwtKeyset,
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var firebaseToken = context.SecurityToken as JsonWebToken;
            string? firebaseUid = firebaseToken?.Claims.FirstOrDefault(claim => claim.Type == "user_id")?.Value;

            if (!string.IsNullOrEmpty(firebaseUid))
            {
                var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                User? user = await userManager.FindByIdAsync(firebaseUid);

                if (user == null)
                {
                    string? name = firebaseToken?.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "";
                    string? email = firebaseToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                    string? picture = firebaseToken?.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

                    user = new User
                    {
                        Id = firebaseUid,
                        UserName = firebaseUid,
                        Email = email,
                        Name = name,
                        Avatar = picture,
                    };

                    await userManager.CreateAsync(user);
                }

                IList<string> userRoles = await userManager.GetRolesAsync(user);
                if (context.Principal?.Identity is ClaimsIdentity identity)
                {
                    foreach (string role in userRoles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }
            }
        }
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

string corsPolicy = "LocalFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, builder => builder
        .WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("/api/auth", UriKind.Relative),
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    { "returnSecureToken", new OpenApiBoolean(true) }
                },
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            { 
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme,
                },
                Scheme = "oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            new List<string> { "openid", "email", "profile" }
        }
    });
});

// Dependency Injection
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<INeighborhoodService, NeighborhoodService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped(typeof(ILikeService<Comment>), typeof(CommentService));
builder.Services.AddScoped(typeof(ILikeService<Post>), typeof(PostService));
builder.Services.AddScoped(typeof(IUserReference), typeof(Comment));
builder.Services.AddScoped(typeof(IUserReference), typeof(Post));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped<NeighborhoodContext>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
RoleUtils.InitializeRolesAsync(app.Services);

app.Run();
