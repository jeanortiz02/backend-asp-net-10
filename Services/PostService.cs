using System;
using Backend.Dtos;

namespace Backend.Services;

public class PostService : IPostService
{
    private HttpClient _httpClient;
    private IPostService _postService;

    public PostService(IPostService postService)
    {
        _httpClient = new HttpClient();
        _postService = postService;
    }

    public async Task<IEnumerable<PostDto>> Get()
    {
        string url = "https://jsonplaceholder.typicode.com/posts";
        var result = await _httpClient.GetAsync(url);
        var body = await result.Content.ReadAsStringAsync();
    }
}
