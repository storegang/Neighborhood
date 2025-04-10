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

builder.Configuration.Sources.Clear();
DotEnv.Load();
builder.Configuration.AddEnvironmentVariables();

string jwtIssuer = builder.Configuration.GetValue<string>("AUTH_DOMAIN") ?? throw new ArgumentNullException("AUTH_DOMAIN");
string jwtAudience = builder.Configuration.GetValue<string>("AUTH_AUDIENCE") ?? throw new ArgumentNullException("AUTH_AUDIENCE");

HttpClient client = new();
string googleKeys = client.GetStringAsync("https://www.googleapis.com/robot/v1/metadata/x509/securetoken@system.gserviceaccount.com").Result;
IEnumerable<SecurityKey> jwtKeyset = new JsonWebKeySet(googleKeys).GetSigningKeys();


if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<NeighborhoodContext>().AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy(UserRoles.BoardAdmin, p => p.RequireRole(UserRoles.BoardAdmin));
    //options.AddPolicy(UserRoles.BoardMember, p => p.RequireRole(UserRoles.BoardMember, UserRoles.BoardAdmin));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer((Action<JwtBearerOptions>)(options =>
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

                // Create user in database if it doesn't exist
                if (user == null)
                {
                    string? name = firebaseToken?.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "";
                    string? email = firebaseToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                    string? picture = firebaseToken?.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

                    user = new User
                    {
                        Id = firebaseUid,
                        UserName = firebaseUid, // UserName seems required and does not support spaces. Will fail creating user silently.
                        Email = email,

                        Name = name,
                        Avatar = picture,
                    };

                    await userManager.CreateAsync(user);
                }

                // Add users roles to their claim.
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
}));

string? connectionString = builder.Configuration.GetConnectionString("ConnectionString");

builder.Services.AddDbContext<NeighborhoodContext>(options =>
{
    options.UseSqlServer(connectionString);
});

string localFrontendCorsPolicy = "AllowSpecificOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(localFrontendCorsPolicy,
        builder => builder
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

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<INeighborhoodService, NeighborhoodService>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped(typeof(ILikeService<Comment>), typeof(CommentService));
builder.Services.AddScoped(typeof(ILikeService<Post>), typeof(PostService));
builder.Services.AddScoped(typeof(IUserReference), typeof(Comment));
builder.Services.AddScoped(typeof(IUserReference), typeof(Post));

// builder.Services.AddScoped<IGenericChildRepository<GenericChildRepository>>;

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));


builder.Services.AddScoped<NeighborhoodContext>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(localFrontendCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

RoleUtils.InitializeRolesAsync(app.Services);

app.Run();
