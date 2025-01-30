using AutoMapper;
using webapi.Models;
using webapi.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryViewModel, Category>();
        CreateMap<Category, CategoryViewModel>();

        CreateMap<CommentViewModel, Comment>();
        CreateMap<Comment, CommentViewModel>();

        CreateMap<LikeViewModel, Like>();
        CreateMap<Like, LikeViewModel>();

        CreateMap<NeighborhoodViewModel, Neighborhood>();
        CreateMap<Neighborhood, NeighborhoodViewModel>();

        CreateMap<PostViewModel, Post>();
        CreateMap<Post, PostViewModel>();

        CreateMap<UserViewModel, User>();
        CreateMap<User, UserViewModel>();
    }
}
