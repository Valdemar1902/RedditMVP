namespace shared.Model;

public class Post {
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
    public string Author {get; set; }
    public User User { get; set; }
    public int CommentCount { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public Post(string author, string title = "", string content = "", int upvotes = 0) {
        Title = title;
        Content = content;
        Upvotes = upvotes;
        Author = author;
        CreatedAt = DateTime.Now;
    }
    public Post() {
        Id = 0;
        Title = "";
        Content = "";
        Upvotes = 0;
        Author = "";
        CreatedAt = DateTime.Now;

    }

    public override string ToString()
    {
        return $"Id: {Id}, Title: {Title}, Content: {Content}, Upvotes: {Upvotes}, User: {User}";
    }
}