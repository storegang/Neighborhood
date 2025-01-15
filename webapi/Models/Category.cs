namespace webapi.Models
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
