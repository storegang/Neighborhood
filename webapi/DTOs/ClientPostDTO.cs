using webapi.Models;

namespace webapi.DTOs
{
    public class ClientPostDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public string CategoryId { get; set; }
        public IEnumerable<string>? ImageUrls { get; set; }

        public ClientPostDTO() { }

        public ClientPostDTO(Post post)
        {
            Id = post.Id;
            Title = post.Title;
            Description = post.Description;
            CategoryId = post.CategoryId;
            ImageUrls = post.Images;
        }

        public ClientPostDTO(string id, string title, string? description, string categoryId, IEnumerable<string>? imageUrls)
        {
            Id = id;
            Title = title;
            Description = description;
            CategoryId = categoryId;
            ImageUrls = imageUrls;
        }
    }
}
