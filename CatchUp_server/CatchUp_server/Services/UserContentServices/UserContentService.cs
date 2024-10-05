using CatchUp_server.Db;
using CatchUp_server.Models.UserContent;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserViewModel;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel.Design;
using static System.Net.Mime.MediaTypeNames;

namespace CatchUp_server.Services.UserContentServices
{
    public class UserContentService
    {
        private readonly ApiDbContext _context;
        private readonly MediaFoldersService _mediaFoldersService;

        public UserContentService(ApiDbContext context, MediaFoldersService mediaFoldersService)
        {
            _context = context;
            _mediaFoldersService = mediaFoldersService;
        }

        public void setDefaultProfilePic(User user)
        {
            var baseProfilePicUrl = "/defaults/ProfilePictures";
            switch (user.Gender)
            {
                case Gender.Male:
                    user.ProfilePicUrl = $"{baseProfilePicUrl}/male.png";
                    break;

                case Gender.Female:
                    user.ProfilePicUrl = $"{baseProfilePicUrl}/female.png";
                    break;

                case Gender.Other:
                    user.ProfilePicUrl = $"{baseProfilePicUrl}/other.jpg";
                    break;
            }
        }

        public void setDefaultCoverPic(User user)
        {
            user.CoverPicUrl = "/defaults/default-cover.jpg";
        }

        public bool EditProfilePic(string userId, IFormFile file)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            // delete the original or leave it?
            var imgUrl = _mediaFoldersService.UploadFile(userId, "ProfilePictures", file);

            user.ProfilePicUrl = imgUrl;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteProfilePic(string userId, string fileName)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            var result = _mediaFoldersService.DeleteFile(userId, "ProfilePictures", fileName);
            if(result == false)
            {
                return false;
            }

            setDefaultProfilePic(user);
            _context.SaveChanges();
            return true;
        }

        public bool EditCoverPic(string userId, IFormFile file)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            // delete the original or leave it?
            var imgUrl = _mediaFoldersService.UploadFile(userId, "CoverPictures", file);

            user.CoverPicUrl = imgUrl;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteCoverPic(string userId, string fileName)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            var result = _mediaFoldersService.DeleteFile(userId, "CoverPictures", fileName);
            if (result == false)
            {
                return false;
            }

            setDefaultCoverPic(user);
            _context.SaveChanges();
            return true;
        }

        //public Post UploadPost(string userId, string? description, List<MediaContent> mediaContents)
        //{
        //    var post = new Post
        //    {
        //        Userid = userId,
        //        Description = description,
        //        CreatedAt = DateTime.Now,
        //        MediaContents = mediaContents,
        //    };

        //    _context.Posts.Add(post);
        //    _context.SaveChanges();
        //    return post;
        //}

        //public Post DeletePost(int postId)
        //{
        //    var post = _context.Posts.SingleOrDefault(p => p.Id == postId);
        //    if(post != null)
        //    {
        //        _context.Posts.Remove(post);
        //        _context.SaveChanges();
        //    }
        //    return post;
        //}

        //public Post EditPost(int postId, string? description)
        //{

        //}

        //public Story UploadStory(string userId, int mediaContentId)
        //{
        //    var story = new Story
        //    {
        //        UserId = userId,
        //        CreatedAt = DateTime.Now,
        //        ExpiresAt = DateTime.Now.AddHours(24),
        //        MediaContentId = mediaContentId
        //    };

        //    _context.Stories.Add(story);
        //    _context.SaveChanges();
        //    return story;
        //}

        //public Story DeleteStory(int storyId)
        //{
        //    var story = _context.Stories.SingleOrDefault(p => p.Id == storyId);
        //    if (story != null)
        //    {
        //        _context.Stories.Remove(story);
        //        _context.SaveChanges();
        //    }
        //    return story;
        //}

        //public Comment AddComment(string userId, int postId, int? parentCommentId, string text)
        //{
        //    var comment = new Comment
        //    {
        //        UserId = userId,
        //        PostId = postId,
        //        ParentCommentId = parentCommentId,
        //        Text = text,
        //        CreatedAt = DateTime.Now,
        //    };

        //    _context.Comments.Add(comment);
        //    _context.SaveChanges();
        //    return comment;
        //}

        //public Comment DeleteComment(int commentId)
        //{
        //    var comment = _context.Comments.SingleOrDefault(p => p.Id == commentId);
        //    if (comment != null)
        //    {
        //        _context.Comments.Remove(comment);
        //        _context.SaveChanges();
        //    }
        //    return comment;
        //}

        //public Comment EditComment(int commentId, string text)
        //{

        //}

        //public Like AddLike(string userId, int? postId, int? commentId)
        //{
        //    var like = new Like
        //    {
        //        UserId = userId,
        //        PostId = postId,
        //        CommentId = commentId,
        //        LikedAt = DateTime.Now
        //    };

        //    _context.Likes.Add(like);
        //    _context.SaveChanges();
        //    return like;
        //}

        //public Like RemoveLike(int likeId)
        //{
        //    var like = _context.Likes.SingleOrDefault(p => p.Id == likeId);
        //    if (like != null)
        //    {
        //        _context.Likes.Remove(like);
        //        _context.SaveChanges();
        //    }
        //    return like;
        //}
    }
}
