using Microsoft.EntityFrameworkCore;
using RedditMVP.Data;
using RedditMVP.Models;

// setup of dependencies and db / config
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(options =>
{
    options.AddPolicy("allowCors", builder =>
    {
        builder.WithOrigins("https://localhost:5002")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// setup of dependencies and db / config
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=reddit.db"));

// builds the application
var app = builder.Build();
app.UseCors("allowCors");

// create var with scoped services
// database migrate creates db if none exists, and updates with old migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Minimal API Endpoints
// GET retreives last 50 post to a list ordered by creationdate
app.MapGet("/posts", (AppDbContext db) =>
    {
        return db.Posts.OrderByDescending(p => p.CreatedAt).Take(50).ToListAsync();
    });

//GET retreival of specific post
app.MapGet("/posts/{id}", (int id, AppDbContext db) =>
{
    var post = db.Posts.Find(id);
    if (post == null) return Results.NotFound();
    return Results.Json(post);
});

// POST creates post with post object as input.
// saves to db and returns succescode
// example: http://localhost:****/posts
app.MapPost("/posts", (Post post, AppDbContext db) =>
{
    db.Posts.Add(post);
    db.SaveChanges();
    return Results.Created($"/posts/{post.Id}", post);
});

// PUT to update post with ID and upvote
// if found, add 1 or -1 depending on bool sent
// example: http://localhost:****/posts/1/vote?upvote=true
app.MapPut("/posts/{id}/upvote", (int id, AppDbContext db) =>
{
    var post = db.Posts.Find(id);
    if (post == null) return Results.NotFound();
    post.Upvotes += 1;
    db.SaveChanges();
    return Results.Ok(post);
});

// GET comments for a specific post using id
app.MapGet("/posts/{id}/comments", (int id, AppDbContext db) =>
{
    var post = db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
    return post != null ? Results.Ok(post.Comments) : Results.NotFound();
});

// POST comment to a specifci post using id
app.MapPost("/posts/{id}/comments", (int id, Comment comment, AppDbContext db) =>
{
    var post = db.Posts.Find(id);
    if (post == null) return Results.NotFound();
    comment.PostId = id;
    db.Comments.Add(comment);
    db.SaveChanges();
    return Results.Created($"/comments/{comment.Id}", comment);
});

// run the app
app.Run();
