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

        [HttpGet("post-by-id"), Authorize]
        public IActionResult GetPost(int postId)
        {
            var post = _postService.GetPost(postId);
            return (post != null) ? Ok(post) : NotFound();
        }

        [HttpPost, Authorize]
        public IActionResult UploadPost([FromForm] UploadPostViewModel postModel, [FromForm] List<IFormFile> files)
        {
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
