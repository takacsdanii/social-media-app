using CatchUp_server.Interfaces.UserContentServices;
using CatchUp_server.ViewModels.UserContentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatchUp_server.Controllers.UserContentControllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("like-post"), Authorize]
        public IActionResult LikePost(string userId, int postId)
        {
            var result = _likeService.LikePost(userId, postId);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost("like-comment"), Authorize]
        public IActionResult LikeComment(string userId, int postId, int commentId)
        {
            var result = _likeService.LikeComment(userId, postId, commentId);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpDelete, Authorize]
        public IActionResult RemoveLike(int id)
        {
            var result = _likeService.RemoveLike(id);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpGet("likers-for-post"), Authorize]
        public IEnumerable<LikeViewModel> GetLikersForPost(int postId)
        {
            return _likeService.GetLikersForPost(postId);
        }

        [HttpGet("likers-for-comment"), Authorize]
        public IEnumerable<LikeViewModel> GetLikersForComment(int postId, int commentId)
        {
            return _likeService.GetLikersForComment(postId, commentId);
        }

        [HttpGet("has-user-liked-content"), Authorize]
        public IActionResult GetLikeIdForPost(string userId, int postId, int? commentId)
        {
            var result = _likeService.GetLikeId(userId, postId, commentId);
            return Ok(result);
        }
    }
}
