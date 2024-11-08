using CatchUp_server.Models.UserContent;
using CatchUp_server.Services.UserContentServices;
using CatchUp_server.ViewModels.UserContentViewModels;
using CatchUp_server.ViewModels.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatchUp_server.Controllers.UserContentControllers
{
    [Route("api/user-content/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet("get-posts-of-user"), Authorize]
        public IEnumerable<PostViewModel> GetPostsOfUser(string userId)
        {
            return _postService.GetPostsOfUser(userId);
        }

        [HttpGet("get-visible-posts-of-user"), Authorize]
        public IEnumerable<PostViewModel> GetPostsOfUser(string postOwnerId, string loggedInUserId)
        {
            return _postService.GetPostsOfUser(postOwnerId, loggedInUserId);
        }

        [HttpGet("post-by-id"), Authorize]
        public IActionResult GetPost(int postId)
        {
            var post = _postService.GetPost(postId);
            return (post != null) ? Ok(post) : NotFound();
        }

        [HttpPost, Authorize]
        public IActionResult UploadPost([FromForm] UploadPostViewModel postModel, [FromForm] List<IFormFile> files)
        {
            const long maxFileSize = 15 * 1024 * 1024; 

            foreach (var file in files)
            {
                if (file.Length > maxFileSize)
                {
                    var errors = $"File size exceeds the 15 MB limit: {file.FileName}";
                    return BadRequest(new { errors });
                }
            }

            var post = _postService.UploadPost(postModel, files);
            return (post != null) ? Ok(post) : NotFound();
        }

        [HttpPut("description"), Authorize]
        public IActionResult EditDescription(int postId, string? description)
        {
            var newDescription = _postService.EditDescription(postId, description);
            if(newDescription == "ERROR_Post_Not_Found") return NotFound();
            return (newDescription != null) ? Ok(newDescription) : Ok();
        }

        [HttpPut("visibility"), Authorize]
        public IActionResult EditVisibility(int postId, Visibility visibility)
        {
            var newVisibility = _postService.EditVisibility(postId, visibility);
            return (newVisibility != null) ? Ok(newVisibility) : NotFound();
        }

        [HttpDelete, Authorize]
        public IActionResult Delete(int postId)
        {
            var result = _postService.Delete(postId);
            return (result) ? Ok() : NotFound();
        }

        [HttpGet("posts-of-followed-users"), Authorize]
        public IEnumerable<PostViewModel> GetPostsOfFollowedUsers(string userId)
        {
            return _postService.GetPostsOfFollowedUsers(userId);
        }
    }
}
