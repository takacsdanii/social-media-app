using CatchUp_server.Db;
using CatchUp_server.Models.UserContent;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserContentViewModels;
using CatchUp_server.ViewModels.UserViewModel;
using Microsoft.EntityFrameworkCore;

namespace CatchUp_server.Services.UserContentServices
{
    public class PostService
    {
        private readonly ApiDbContext _context;
        private readonly MediaFoldersService _mediaFoldersService;

        private const string postsFolder = "Posts";

        public PostService(ApiDbContext context, MediaFoldersService mediaFoldersService)
        {
            _context = context;
            _mediaFoldersService = mediaFoldersService;
        }

        //private PostViewModel MapPostToViewModel(Post p)
        //{
        //    return new PostViewModel
        //    {
        //        Id = p.Id,
        //        Description = p.Description,
        //        Visibility = p.Visibility,
        //        CreatedAt = p.CreatedAt,
        //        UserId = p.Userid,
        //        MediaUrls = p.MediaContents.Select(mc => mc.MediaUrl).ToList(),
        //        LikeCount = p.Likes.Count,
        //        CommentCount = p.Comments.Count
        //    };
        //}

        public IReadOnlyCollection<PostViewModel> GetPostsOfUser(string userId)
        {
            //var posts = _context.Posts
            //    .Include(p => p.MediaContents)
            //    .Include(p => p.Likes)
            //    .Include(p => p.Comments)
            //    .Where(p => p.Userid == userId).ToList();

            //return posts.Select(p => MapPostToViewModel(p)).ToList();

            return _context.Posts
                .Where(p => p.Userid == userId)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UserId = p.Userid,
                    MediaUrls = p.MediaContents.Select(mc => mc.MediaUrl).ToList(),
                    LikeCount = _context.Likes.Count(l => l.PostId == p.Id && l.CommentId == null),
                    CommentCount = _context.Comments.Count(c => c.PostId == p.Id && c.ParentCommentId == null)
                })
                .ToList();
        }

        public PostViewModel GetPost(int postId)
        {
            //var post = _context.Posts
            //    .Include(p => p.MediaContents)
            //    .Include(p => p.Likes)
            //    .Include(p => p.Comments)
            //    .SingleOrDefault(p => p.Id == postId);

            //return post != null ? MapPostToViewModel(post) : null;

            return _context.Posts
                .Where(p => p.Id == postId)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UserId = p.Userid,
                    MediaUrls = p.MediaContents.Select(mc => mc.MediaUrl).ToList(),
                    LikeCount = _context.Likes.Count(l => l.PostId == p.Id && l.CommentId == null),
                    CommentCount = _context.Comments.Count(c => c.PostId == p.Id && c.ParentCommentId == null)
                })
                .SingleOrDefault();
        }

        public PostViewModel UploadPost(UploadPostViewModel postModel, List<IFormFile> files)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == postModel.UserId);
            if (user == null)
            {
                return null;
            }

            var post = new Post
            {
                Userid = postModel.UserId,
                Description = postModel.Description,
                Visibility = postModel.Visibility,
                CreatedAt = DateTime.Now,
                MediaContents = new List<MediaContent>(),
                Likes = new List<Like>(),
                Comments = new List<Comment>()
            };

            _context.Posts.Add(post);
            _context.SaveChanges();

            var postViewModel = new PostViewModel
            {
                Id = post.Id,
                Description = post.Description,
                CreatedAt = post.CreatedAt,
                Visibility = post.Visibility,
                UserId = post.Userid,
                MediaUrls = new List<string>(),
                LikeCount = 0,
                CommentCount = 0
            };

            foreach (var file in files)
            {
                string mediaUrl = _mediaFoldersService.UploadFile(user.Id, postsFolder, file);
                var mediaContent = new MediaContent
                {
                    MediaUrl = mediaUrl,
                    Type = GetMediaType(file),
                    StoryId = null,
                    PostId = post.Id,
                };

                _context.MediaContents.Add(mediaContent);
                post.MediaContents.Add(mediaContent);

                postViewModel.MediaUrls.Add(mediaUrl);
            }

            _context.SaveChanges();
            return postViewModel;
        }

        private MediaType GetMediaType(IFormFile file)
        {
            var contentType = file.ContentType.ToLower();
            if (contentType.Contains("image")) return MediaType.Image;
            if (contentType.Contains("video")) return MediaType.Video;
            return MediaType.Other;
        }

        public string EditDescription(int postId, string? description)
        {
            var post = _context.Posts.SingleOrDefault(p => p.Id == postId);
            if (post == null)
            { 
                return "ERROR_Post_Not_Found!";
            }

            post.Description = description;
            _context.SaveChanges();

            return post.Description;
        }

        public Visibility? EditVisibility(int postId, Visibility visibility)
        {
            var post = _context.Posts.SingleOrDefault(p => p.Id == postId);
            if (post == null)
            {
                return null;
            }

            post.Visibility = visibility;
            _context.SaveChanges();

            return post.Visibility;
        }

        public bool Delete(int postId)
        {
            var post = _context.Posts.Include(p => p.MediaContents).SingleOrDefault(p => p.Id == postId);
            if (post == null)
            {
                return false;
            }

            foreach(var content in post.MediaContents)
            {
                string fileName = Path.GetFileName(content.MediaUrl);
                _mediaFoldersService.DeleteFile(post.Userid, postsFolder, fileName);
            }

            var likes = _context.Likes.Where(l => l.PostId == postId);
            _context.Likes.RemoveRange(likes);

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return true;
        }
    }
}
