using CatchUp_server.Db;
using CatchUp_server.Models.UserContent;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserContentViewModels;
using Microsoft.EntityFrameworkCore;

namespace CatchUp_server.Services.UserContentServices
{
    public class LikeService
    {
        private readonly ApiDbContext _context;

        public LikeService(ApiDbContext context)
        {
            _context = context;
        }

        private int? LikeEntity(string userId, int postId, int? commentId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            var post = _context.Posts
                .Include(p => p.Likes)
                .SingleOrDefault(p => p.Id == postId);
            if (post == null)
            {
                return null;
            }

            var existingLike = _context.Likes.SingleOrDefault(l => l.UserId == userId && l.PostId == postId && l.CommentId == commentId);
            if (existingLike != null)
            {
                return null;
            }

            Comment comment = null; 
            if(commentId != null) 
            {
                comment = _context.Comments.SingleOrDefault(c => c.Id == commentId);
                if (comment == null)
                {
                    return null;
                }
            }

            var like = new Like
            {
                LikedAt = DateTime.Now,
                UserId = userId,
                PostId = postId,
                CommentId = commentId
            };

            if(commentId == null)
            {
                post.Likes.Add(like);
            }
            else
            {
                comment.Likes.Add(like);
            }

            _context.Likes.Add(like);
            _context.SaveChanges();

            return like.Id;
        }

        public int? LikePost(string userId, int postId)
        {
            return LikeEntity(userId, postId, null);
        }

        public int? LikeComment(string userId, int postId, int commentId)
        {
            return LikeEntity(userId, postId, commentId);
        }

        public int? RemoveLike(int likeId)
        {
            var like = _context.Likes.SingleOrDefault(l => l.Id == likeId);
            if(like == null)
            {
                return null;
            }

            _context.Likes.Remove(like);
            _context.SaveChanges();
            return like.Id;
        }
    }
}
