using CatchUp_server.Services.UserContentServices;
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

        [HttpPost("comment-post")]
        public IActionResult AddCommentToPost(string userId, int postId, string text)
        {
            var result = _commentService.AddCommentToPost(userId, postId, text);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost("comment-comment")]
        public IActionResult AddCommentToComment(string userId, int postId, int parentCommentId, string text)
        {
            var result = _commentService.AddCommentToComment(userId, postId, parentCommentId, text);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpDelete]
        public IActionResult DeleteComment(int id)
        {
            var result = _commentService.DeleteComment(id);
            return (result != null) ? Ok(result) : NotFound();
        }
    }
}
