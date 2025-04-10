using webapi.Models;

namespace webapi.DTOs
{
    public class ClientCommentDTO
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ParentPostId { get; set; }

        public string? ImageUrl { get; set; }

        public ClientCommentDTO() { }

        public ClientCommentDTO(Comment comment)
        {
            Id = comment.Id;
            Content = comment.Content;
            ParentPostId = comment.ParentPostId;
            ImageUrl = comment.ImageUrl;
        }

        public ClientCommentDTO(string id, string content, string parentPostId, string? imageUrl)
        {
            Id = id;
            Content = content;
            ParentPostId = parentPostId;
            ImageUrl = imageUrl;
        }
    }
}
