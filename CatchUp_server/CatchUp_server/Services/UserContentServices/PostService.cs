using CatchUp_server.Db;
using CatchUp_server.Models.UserContent;
using CatchUp_server.Services.AuthServices;
using CatchUp_server.Services.FriendsServices;
using CatchUp_server.ViewModels.UserContentViewModels;
using Microsoft.EntityFrameworkCore;

namespace CatchUp_server.Services.UserContentServices
{
    public class PostService
    {
        private readonly ApiDbContext _context;
        private readonly MediaFoldersService _mediaFoldersService;
        private readonly FriendsService _friendsService;

        private const string postsFolder = "Posts";

        public PostService(ApiDbContext context, MediaFoldersService mediaFoldersService, FriendsService friendsService, AuthService authService)
        {
            _context = context;
            _mediaFoldersService = mediaFoldersService;
            _friendsService = friendsService;
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
                    MediaContents = p.MediaContents,
                    LikeCount = _context.Likes.Count(l => l.PostId == p.Id && l.CommentId == null),
                    CommentCount = _context.Comments.Count(c => c.PostId == p.Id)
                })
                .SingleOrDefault();
        }

        public IReadOnlyCollection<PostViewModel> GetPostsOfUser(string userId)
        {
            return _context.Posts
                .Where(p => p.Userid == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UserId = p.Userid,
                    //MediaUrls = p.MediaContents.Select(mc => mc.MediaUrl).ToList(),
                    MediaContents = p.MediaContents,
                    LikeCount = _context.Likes.Count(l => l.PostId == p.Id && l.CommentId == null),
                    CommentCount = _context.Comments.Count(c => c.PostId == p.Id)
                })
                .ToList();
        }

        public IReadOnlyCollection<PostViewModel> GetPostsOfUser(string postOwnerId, string loggedInUserId)
        {
            //var posts = GetPostsOfUser(postOwnerId);
            if (loggedInUserId == postOwnerId)
            {
                return GetPostsOfUser(postOwnerId); //posts;
            }

            var doesLoggedInUserFollowPostOwner = _friendsService.doesUserFollowTargetUser(loggedInUserId, postOwnerId) ?? false;
            var doesPostOwnerFollowLoggedInUser = _friendsService.doesUserFollowTargetUser(postOwnerId, loggedInUserId) ?? false;

            //return posts
            return _context.Posts
                .Where(
                    p => p.Userid == postOwnerId
                    && 
                    (p.Visibility == Visibility.Public
                    || (doesLoggedInUserFollowPostOwner && doesPostOwnerFollowLoggedInUser && p.Visibility == Visibility.Friends)
                    || (doesLoggedInUserFollowPostOwner && p.Visibility == Visibility.Followers)
                    || (doesPostOwnerFollowLoggedInUser && p.Visibility == Visibility.Following))
                )
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UserId = p.Userid,
                    MediaContents = p.MediaContents,
                    LikeCount = _context.Likes.Count(l => l.PostId == p.Id && l.CommentId == null),
                    CommentCount = _context.Comments.Count(c => c.PostId == p.Id)
                })
                .ToList();
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
                MediaContents = new List<MediaContent>(),
                LikeCount = 0,
                CommentCount = 0
            };

            foreach (var file in files)
            {
                string mediaUrl = _mediaFoldersService.UploadFile(user.Id, postsFolder, file);
                var mediaContent = new MediaContent
                {
                    MediaUrl = mediaUrl,
                    Type = _mediaFoldersService.GetMediaType(file),
                    PostId = post.Id,
                };

                _context.MediaContents.Add(mediaContent);
                post.MediaContents.Add(mediaContent);

                postViewModel.MediaContents.Add(mediaContent);
            }

            _context.SaveChanges();
            return postViewModel;
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

        public IReadOnlyCollection<PostViewModel> GetPostsOfFollowedUsers(string userId)
        {
            var followings = _friendsService.GetFollowing(userId);
            var userIds = followings.Select(f => f.Id).ToList();

            var friends = _friendsService.GetFriends(userId);
            var friendIds = friends.Select(f => f.Id).ToList();

            return _context.Posts
                .Where(
                    p => userIds.Contains(p.Userid)
                    &&
                    (p.Visibility == Visibility.Public
                    || p.Visibility == Visibility.Followers
                    || (friendIds.Contains(p.Userid)
                       &&
                       (p.Visibility == Visibility.Friends
                       || p.Visibility == Visibility.Following))
                    )
                )
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UserId = p.Userid,
                    MediaContents = p.MediaContents,
                    LikeCount = _context.Likes.Count(l => l.PostId == p.Id && l.CommentId == null),
                    CommentCount = _context.Comments.Count(c => c.PostId == p.Id)
                })
                .ToList();
        }
    }
}
