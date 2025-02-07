using AutoMapper;
using webapi.Models;
using webapi.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryDTO, Category>();
        CreateMap<Category, CategoryDTO>();

        CreateMap<CommentDTO, Comment>();
        CreateMap<Comment, CommentDTO>();

        CreateMap<LikeDTO, Like>();
        CreateMap<Like, LikeDTO>();

        CreateMap<NeighborhoodDTO, Neighborhood>();
        CreateMap<Neighborhood, NeighborhoodDTO>();

        CreateMap<PostDTO, Post>();
        CreateMap<Post, PostDTO>();

        CreateMap<UserDTO, User>();
        CreateMap<User, UserDTO>();
    }
}
