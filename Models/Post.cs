namespace RedditMVP.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Upvotes { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Comment> Comments {get; set; } = new();
}
