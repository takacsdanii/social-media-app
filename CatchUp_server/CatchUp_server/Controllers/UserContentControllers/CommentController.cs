using CatchUp_server.Services.UserContentServices;
using CatchUp_server.ViewModels.UserContentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatchUp_server.Controllers.UserContentControllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("comment-post"), Authorize]
        public IActionResult AddCommentToPost(string userId, int postId, string text)
        {
            var result = _commentService.AddCommentToPost(userId, postId, text);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost("comment-comment"), Authorize]
        public IActionResult AddReplyToComment(string userId, int postId, int parentCommentId, string text)
        {
            var result = _commentService.AddReplyToComment(userId, postId, parentCommentId, text);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpDelete, Authorize]
        public IActionResult DeleteComment(int id)
        {
            var result = _commentService.DeleteComment(id);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpGet("comments-for-post"), Authorize]
        public IEnumerable<CommentViewModel> GetCommentsForPost(int postId)
        {
            return _commentService.GetCommentsForPost(postId);
        }

        [HttpGet("replies-for-comment"), Authorize]
        public IEnumerable<CommentViewModel> GetRepliesForComment(int postId, int parentCommentId)
        {
            return _commentService.GetRepliesForComment(postId, parentCommentId);
        }

        [HttpGet, Authorize]
        public IActionResult GetCommentById(int id)
        {
            var comment = _commentService.GetCommentById(id);
            return (comment != null) ? Ok(comment) : NotFound();
        }
    }
}
