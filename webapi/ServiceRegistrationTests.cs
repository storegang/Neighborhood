using webapi.Migrations;

using Microsoft.Extensions.DependencyInjection;
using webapi.DataContexts;
using webapi.Repositories;
using webapi.Services;
using Xunit;

namespace webapi.Tests;
public class ServiceRegistrationTests
{
    [Fact]
    public void TestServiceRegistrations()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = WebApplication.CreateBuilder();

        // Register DbContextOptions
        services.AddDbContext<NeighborhoodContext>();

        // Register services as in Program.cs
        services.AddScoped<CategoryService>();
        services.AddScoped<CommentService>();
        services.AddScoped<LikeService>();
        services.AddScoped<NeighborhoodService>();
        services.AddScoped<PostService>();
        services.AddScoped<UserService>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Register AutoMapper with the required services
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, typeof(Program));

        // Act
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<CategoryService>());
        Assert.NotNull(serviceProvider.GetService<CommentService>());
        Assert.NotNull(serviceProvider.GetService<LikeService>());
        Assert.NotNull(serviceProvider.GetService<NeighborhoodService>());
        Assert.NotNull(serviceProvider.GetService<PostService>());
        Assert.NotNull(serviceProvider.GetService<UserService>());

        Assert.NotNull(serviceProvider.GetService<ICategoryRepository>());
        Assert.NotNull(serviceProvider.GetService<ICommentRepository>());
        Assert.NotNull(serviceProvider.GetService<ILikeRepository>());
        Assert.NotNull(serviceProvider.GetService<INeighborhoodRepository>());
        Assert.NotNull(serviceProvider.GetService<IPostRepository>());
        Assert.NotNull(serviceProvider.GetService<IUserRepository>());

        Assert.NotNull(serviceProvider.GetService<NeighborhoodContext>());
    }
}
