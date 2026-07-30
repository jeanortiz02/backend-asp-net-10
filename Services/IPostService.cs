using System;
using Backend.Dtos;

namespace Backend.Services;

public interface IPostService
{
    public Task<IEnumerable<PostDto>> Get();
}
