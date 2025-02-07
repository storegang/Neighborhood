using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using webapi.Models;
using webapi.Services;
using webapi.ViewModels;

public class MappingProfile : Profile
{
    private readonly NeighborhoodService _neighborhoodService;
    private readonly PostService _postService;
    private readonly UserService _userService;

    public MappingProfile()
    {
        CreateMap<CategoryViewModel, Category>()
            .ForMember(dest => dest.Neighborhood, opt => opt.MapFrom(src => _neighborhoodService.GetNeighborhoodById(src.NeighborhoodId)))
            .ForMember(dest => dest.Posts, opt => opt.Ignore());
        CreateMap<Category, CategoryViewModel>()
            .ForMember(dest => dest.NeighborhoodId, opt => opt.MapFrom(src => src.Neighborhood.Id))
            .ForMember(dest => dest.Posts, opt => opt.Ignore());

        CreateMap<CommentViewModel, Comment>()
            .ForMember(dest => dest.ParentPost, opt => opt.MapFrom(src => _postService.GetPostById(src.ParentPostId)))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
        CreateMap<Comment, CommentViewModel>()
            .ForMember(dest => dest.ParentPostId, opt => opt.MapFrom(src => src.ParentPost.Id))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        CreateMap<LikeViewModel, Like>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => _userService.GetUserById(src.UserId)))
            .ForMember(dest => dest.Post, opt => opt.MapFrom(src => _postService.GetPostById(src.PostId)));
        CreateMap<Like, LikeViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Post.Id));

        CreateMap<NeighborhoodViewModel, Neighborhood>()
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForMember(dest => dest.Users, opt => opt.Ignore());
        CreateMap<Neighborhood, NeighborhoodViewModel>()
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForMember(dest => dest.Users, opt => opt.Ignore());

        CreateMap<PostViewModel, Post>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Likes, opt => opt.Ignore());
        CreateMap<Post, PostViewModel>()
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.LikedByUserId, opt => opt.Ignore());

        CreateMap<UserViewModel, User>();
        CreateMap<User, UserViewModel>();
    }
}
