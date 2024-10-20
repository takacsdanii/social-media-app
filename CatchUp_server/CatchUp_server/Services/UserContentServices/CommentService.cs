using CatchUp_server.Db;
using CatchUp_server.Models.UserContent;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using static System.Net.Mime.MediaTypeNames;

namespace CatchUp_server.Services.UserContentServices
{
    public class CommentService
    {
        private readonly ApiDbContext _context;

        public CommentService(ApiDbContext context)
        {
            _context = context;
        }

        private int? AddComment(string userId, int postId, int? parentCommentId, string text)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            var post = _context.Posts
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.Id == postId);
            if (post == null)
            {
                return null;
            }

            Comment parentComment = null;
            if(parentCommentId != null)
            {
                parentComment = _context.Comments.SingleOrDefault(c => c.Id == parentCommentId);
                if (parentComment == null)
                {
                    return null;
                }
            }

            var comment = new Comment
            {
                Text = text,
                CreatedAt = DateTime.Now,
                UserId = userId,
                PostId = postId,
                ParentCommentId = parentCommentId,
                Likes = new List<Like>(),
                Replies = new List<Comment>()
            };

            if (parentCommentId == null)
            {
                post.Comments.Add(comment);
            }
            else
            {
                parentComment.Replies.Add(comment);
            }

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return comment.Id;
        }

        public int? AddCommentToPost(string userId, int postId, string text)
        {
            return AddComment(userId, postId, null, text);
        }

        public int? AddCommentToComment(string userId, int postId, int parentCommentId, string text)
        {
            return AddComment(userId, postId, parentCommentId, text);
        }

        public int? DeleteComment(int commentId)
        {
            var comment = _context.Comments.SingleOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return null;
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return comment.Id;
        }
    }
}
