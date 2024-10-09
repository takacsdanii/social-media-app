using CatchUp_server.Db;
using CatchUp_server.Models.UserContent;
using CatchUp_server.ViewModels.UserContentViewModels;
using CatchUp_server.ViewModels.UserViewModel;
using Microsoft.EntityFrameworkCore;

namespace CatchUp_server.Services.UserContentServices
{
    public class PostService
    {
        private readonly ApiDbContext _context;
        private readonly MediaFoldersService _mediaFoldersService;

        public PostService(ApiDbContext context, MediaFoldersService mediaFoldersService)
        {
            _context = context;
            _mediaFoldersService = mediaFoldersService;
        }

        //public PostViewModel MapPostToViewModel(Post post)
        //{
        //    return new PostViewModel
        //    {
        //        Id = post.Id,
        //        Description = post.Description,
        //        Visibility = post.Visibility,
        //        CreatedAt = post.CreatedAt,
        //        MediaUrls = post.MediaContents.Select(mc => mc.MediaUrl).ToList()
        //    };
        //}

        public IReadOnlyCollection<PostViewModel> GetPostsOfUser(string userId)
        {
            return _context.Posts
                .Where(p => p.User.Id == userId)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UserId = p.Userid,
                    MediaUrls = p.MediaContents.Select(mc => mc.MediaUrl).ToList()
                })
                .ToList();
        }

        public PostViewModel GetPost(int postId)
        {
            return _context.Posts
                .Where(p => p.Id == postId)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UserId = p.Userid,
                    MediaUrls = p.MediaContents.Select(mc => mc.MediaUrl).ToList()
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
                MediaContents = new List<MediaContent>()
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
                MediaUrls = new List<string>()
            };

            foreach (var file in files)
            {
                string mediaUrl = _mediaFoldersService.UploadFile(user.Id, "Posts", file);
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
    }
}
