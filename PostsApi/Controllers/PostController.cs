using System.Net;
using Microsoft.AspNetCore.Mvc;
using Posts.Application.Repositories;
using PostsApi.Contracts.Requests;
using PostsApi.Mapping;

namespace PostsApi.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    [HttpGet(ApiEndpoints.Posts.GetAll)]
    public async Task<ActionResult> GetALlPosts()
    {
        var posts = await _postRepository.GetAllAsync();
        return Ok(posts.MapToResponse());
    }

    [HttpPost(ApiEndpoints.Posts.Create)]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest newPost)
    {
        var post = newPost.MapToPost();
        var created = await _postRepository.CreateAsync(post);

        if (!created)
        {
            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Content = "We ran into a issue creating the provided post",
                ContentType = "application/json"
            };
        }

        return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Posts.GetById)]
    public async Task<IActionResult> GetPostById([FromRoute] Guid id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post is null)
        {
            return NotFound();
        }

        return Ok(post.MapToResponse());
    }

    [HttpPut(ApiEndpoints.Posts.Update)]
    public async Task<IActionResult> UpdatePostById(
        [FromRoute] Guid id,
        [FromBody] UpdatePostRequest post
    )
    {
        var updatedPost = post.MapToPost(id);
        var updated = await _postRepository.UpdateAsync(updatedPost);

        if (!updated)
        {
            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Content = "We ran into a issue creating the provided post",
                ContentType = "application/json"
            };
        }

        return Ok(updatedPost.MapToResponse());
    }

    [HttpDelete(ApiEndpoints.Posts.Delete)]
    public async Task<IActionResult> DeletePost([FromRoute] Guid id)
    {
        var foundPost = _postRepository.GetByIdAsync(id);
        var postWasDeleted = await _postRepository.DeleteByIdAsync(id);

        if (!postWasDeleted)
        {
            NotFound();
        }
        
        return Ok(await foundPost);
    }

    [HttpGet(ApiEndpoints.Posts.Comments.GetAllByPostId)]
    public async Task<IActionResult> GetCommentsByPostId([FromRoute] Guid id)
    {
        var comments = await _postRepository.GetCommentsByPostIdAsync(id);
        return Ok(comments);
    }
}

