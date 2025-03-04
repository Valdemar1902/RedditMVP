namespace RedditMVP.Models;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string? Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Votes { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
