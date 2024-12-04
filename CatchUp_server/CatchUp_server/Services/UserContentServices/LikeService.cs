using CatchUp_server.Db;
using CatchUp_server.Interfaces.UserContentServices;
using CatchUp_server.Models.UserContent;
using CatchUp_server.ViewModels.UserContentViewModels;
using Microsoft.EntityFrameworkCore;

namespace CatchUp_server.Services.UserContentServices
{
    public class LikeService : ILikeService
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
                comment = _context.Comments
                    .Include(c => c.Likes)
                    .SingleOrDefault(c => c.Id == commentId);
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

            if (commentId == null)
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

        public IReadOnlyCollection<LikeViewModel> GetLikersForPost(int postId)
        {
            return _context.Likes
                .Where(l => l.PostId == postId && l.CommentId == null)
                .Select(l => new LikeViewModel
                {
                    Id = l.Id,
                    LikedAt = l.LikedAt,
                    PostId = l.PostId,
                    UserId = l.UserId,
                    UserName = l.User.UserName,
                    ProfilePicUrl = l.User.ProfilePicUrl
                })
                .ToList();
        }

        public IReadOnlyCollection<LikeViewModel> GetLikersForComment(int postId, int commentId)
        {
            return _context.Likes
                .Where(l => l.PostId == postId && l.CommentId == commentId)
                .Select(l => new LikeViewModel
                {
                    Id = l.Id,
                    LikedAt = l.LikedAt,
                    PostId = l.PostId,
                    UserId = l.UserId,
                    UserName = l.User.UserName,
                    ProfilePicUrl = l.User.ProfilePicUrl
                })
                .ToList();
        }

        public int GetLikeId(string userId, int postId, int? commentId)
        {
            return _context.Likes
                .Where(l => l.UserId == userId && l.PostId == postId && l.CommentId == commentId)
                .Select(l => l.Id)
                .SingleOrDefault();
        }
    }
}
