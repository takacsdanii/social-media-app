using CatchUp_server.Services.UserContentServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatchUp_server.Controllers.UserContentControllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly LikeService _likeService;

        public LikeController(LikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("like-post")]
        public IActionResult LikePost(string userId, int postId)
        {
            var result = _likeService.LikePost(userId, postId);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost("like-comment")]
        public IActionResult LikeComment(string userId, int postId, int commentId)
        {
            var result = _likeService.LikeComment(userId, postId, commentId);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpDelete]
        public IActionResult RemoveLike(int id)
        {
            var result = _likeService.RemoveLike(id);
            return (result != null) ? Ok(result) : NotFound();
        }
    }
}
