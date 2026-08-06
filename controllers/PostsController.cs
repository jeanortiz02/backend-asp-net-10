using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        IPostsService _titleService;
        public PostsController( IPostsService postsService)
        {
            this._titleService = postsService;
        }


        [HttpGet]
        public async Task<IEnumerable<PostDto>> Get() => await this._titleService.Get();

    }
}
