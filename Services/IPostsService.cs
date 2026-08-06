using System;
using Backend.Dtos;

namespace Backend.Services;

public interface IPostsService
{
    public Task<IEnumerable<PostDto>> Get();
}
