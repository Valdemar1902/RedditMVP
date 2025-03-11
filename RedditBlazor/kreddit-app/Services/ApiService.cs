using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

using shared.Model;

namespace kreddit_app.Data;

public class ApiService
{
    private readonly HttpClient http;
    private readonly IConfiguration configuration;
    private readonly string baseAPI = "";

    public ApiService(HttpClient http, IConfiguration configuration)
    {
        this.http = http;
        this.configuration = configuration;
        this.baseAPI = configuration["base_api"];
    }

    public async Task<Post[]> GetPosts()
    {
        string url = $"{baseAPI}posts/";
        return await http.GetFromJsonAsync<Post[]>(url);
    }

    public async Task<Post> GetPost(int id)
    {
        string url = $"{baseAPI}posts/{id}";
        return await http.GetFromJsonAsync<Post>(url);
    }

    public async Task<List<Comment>> GetComments(int id)
    {
        string url = $"{baseAPI}posts/{id}/comments";
        return await http.GetFromJsonAsync<List<Comment>>(url) ?? new List<Comment>();
    }

    public async Task<Comment> CreateComment(string content, string author, int postId, int userId)
    {
        string url = $"{baseAPI}posts/{postId}/comments";
     
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { content, author, userId });

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Comment object
        Comment? newComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new comment 
        return newComment;
    }

    public async Task<Post> UpvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/upvote";

        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        string json = await msg.Content.ReadAsStringAsync();

        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return updatedPost;


    }

    public async Task<Post?> CreatePost(string title, string content, string author)
    {
        string url = $"{baseAPI}posts";

        var newPost = new Post(author, title, content, 0);

        HttpResponseMessage response = await http.PostAsJsonAsync(url, newPost);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error creating post: {response.StatusCode} - {errorMessage}");
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();

        try
        {
            return JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            Console.WriteLine($"Response Content: {json}");
            return null;
        }
    }

    public async Task<bool> DeletePost(int id)
    {
        string url = $"{baseAPI}posts/{id}";

        HttpResponseMessage response = await http.DeleteAsync(url);

        return true;

    }
}
