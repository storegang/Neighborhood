using Microsoft.EntityFrameworkCore;
using dotenv.net;
using webapi.Repositories;
using webapi.Services;
//using FluentValidation;
//using FluentValidation.AspNetCore;
using webapi.DataContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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
    
});

var connectionString = builder.Configuration.GetConnectionString("ConnectionString");

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
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<LikeService>();
builder.Services.AddScoped<NeighborhoodService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<NeighborhoodContext>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(localFrontendCorsPolicy);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
