using Microsoft.EntityFrameworkCore;
using webapi.Controllers;
using webapi.Repositories;
using webapi.Services;
//using FluentValidation;
//using FluentValidation.AspNetCore;
using webapi.DataContexts;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

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
builder.Services.AddScoped<NeighborhoodService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<NeighborhoodContext>();

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

app.MapControllers();

app.Run();
