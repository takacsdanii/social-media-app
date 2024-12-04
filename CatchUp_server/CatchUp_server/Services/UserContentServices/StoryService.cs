using CatchUp_server.Db;
using CatchUp_server.Interfaces;
using CatchUp_server.Interfaces.UserContentServices;
using CatchUp_server.Models.UserContent;
using CatchUp_server.ViewModels.UserContentViewModels;
using CatchUp_server.ViewModels.UserViewModels;
using Microsoft.EntityFrameworkCore;

namespace CatchUp_server.Services.UserContentServices
{
    public class StoryService : IStoryService
    {
        private readonly ApiDbContext _context;
        private readonly IMediaFoldersService _mediaFoldersService;
        private readonly IFriendsService _friendsService;

        private const string storiesFolder = "Stories";

        public StoryService(ApiDbContext context, IMediaFoldersService mediaFoldersService, IFriendsService friendsService)
        {
            _context = context;
            _mediaFoldersService = mediaFoldersService;
            _friendsService = friendsService;
        }

        public StoryViewModel GetStory(int id)
        {
            return _context.Stories
                .Where(s => s.Id == id)
                .Include(s => s.MediaContent)
                .Select(s => new StoryViewModel
                {
                    Id = s.Id,
                    CreatedAt = s.CreatedAt,
                    ExpiresAt = s.ExpiresAt,
                    Visibility = s.Visibility,
                    UserId = s.UserId,
                    MediaContent = s.MediaContent,
                    ViewCount = s.StoryViewers.Count()
                })
            .SingleOrDefault();
        }

        public IReadOnlyCollection<StoryViewModel> GetStoriesOfUser(string userId)
        {
            return _context.Stories
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.CreatedAt)
                //.Include(s => s.MediaContent)
                .Select(s => new StoryViewModel
                {
                    Id = s.Id,
                    CreatedAt = s.CreatedAt,
                    ExpiresAt = s.ExpiresAt,
                    Visibility = s.Visibility,
                    UserId = s.UserId,
                    //MediaUrl = s.MediaContent.MediaUrl,
                    MediaContent = s.MediaContent,
                    ViewCount = s.StoryViewers.Count()
                })
                .ToList();
        }

        public IReadOnlyCollection<StoryViewModel> GetStoriesOfUser(string storyOwnerId, string loggedInUserId)
        {
            if (loggedInUserId == storyOwnerId)
            {
                return GetStoriesOfUser(storyOwnerId);
            }

            var doesLoggedInUserFollowStoryOwner = _friendsService.doesUserFollowTargetUser(loggedInUserId, storyOwnerId) ?? false;
            var doesStoryOwnerFollowLoggedInUser = _friendsService.doesUserFollowTargetUser(storyOwnerId, loggedInUserId) ?? false;

            return _context.Stories
                .Where(
                    s => s.UserId == storyOwnerId
                    &&
                    (s.Visibility == Visibility.Public
                    || (doesLoggedInUserFollowStoryOwner && doesStoryOwnerFollowLoggedInUser && s.Visibility == Visibility.Friends)
                    || (doesLoggedInUserFollowStoryOwner && s.Visibility == Visibility.Followers)
                    || (doesStoryOwnerFollowLoggedInUser && s.Visibility == Visibility.Following))
                )
                .OrderBy(s => s.CreatedAt)
                .Select(s => new StoryViewModel
                {
                    Id = s.Id,
                    CreatedAt = s.CreatedAt,
                    ExpiresAt = s.ExpiresAt,
                    Visibility = s.Visibility,
                    UserId = s.UserId,
                    MediaContent = s.MediaContent,
                    ViewCount = s.StoryViewers.Count()
                })
                .ToList();
        }

        public StoryViewModel UploadStory(UploadStoryViewModel uploadModel)
        {
            if(uploadModel.File == null)
            {
                return null;
            }

            var user = _context.Users.SingleOrDefault(u =>  u.Id == uploadModel.UserId);
            if(user == null)
            {
                return null;
            }

            string mediaUrl = _mediaFoldersService.UploadFile(user.Id, storiesFolder, uploadModel.File);
            var mediaContent = new MediaContent
            {
                MediaUrl = mediaUrl,
                Type = _mediaFoldersService.GetMediaType(uploadModel.File),
                PostId = null
            };

            _context.MediaContents.Add(mediaContent);
            _context.SaveChanges();

            var story = new Story
            {
                UserId = user.Id,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddHours(24),
                Visibility = uploadModel.Visibility,
                StoryViewers = new List<StoryViewer>(),
                MediaContentId = mediaContent.Id
            };

            _context.Stories.Add(story);
            _context.SaveChanges();

            var storyViewModel = new StoryViewModel
            {
                Id = story.Id,
                CreatedAt = story.CreatedAt,
                ExpiresAt= story.ExpiresAt,
                Visibility = story.Visibility,
                UserId = story.UserId,
                MediaContent = mediaContent,
                ViewCount = 0
            };

            return storyViewModel;
        }

        public bool Delete(int storyId)
        {
            var story = _context.Stories
                .Include(s => s.MediaContent)
                .Include(s => s.StoryViewers)
                .SingleOrDefault(s => s.Id == storyId);

            if (story == null)
            {
                return false;
            }

            string fileName = Path.GetFileName(story.MediaContent.MediaUrl);
            _mediaFoldersService.DeleteFile(story.UserId, storiesFolder, fileName);

            _context.StoriesViewer.RemoveRange(story.StoryViewers);
            _context.Stories.Remove(story);
            _context.MediaContents.Remove(story.MediaContent);

            _context.SaveChanges();
            return true;
        }

        public IReadOnlyCollection<UserPreviewViewModel> GetStoryViewers(int id)
        {
            return _context.Stories
                .Where(s => s.Id == id)
                .SelectMany(s => s.StoryViewers)
                .Select(sv => new UserPreviewViewModel
                {
                    Id = sv.UserId,
                    UserName = sv.User.UserName,
                    ProfilePicUrl = sv.User.ProfilePicUrl
                })
                .ToList();
        }

        public StoryViewer AddViewerToStory(string userId, int storyId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if(user == null)
            {
                return null;
            }

            var story = _context.Stories.SingleOrDefault(s => s.Id == storyId);
            if(story == null)
            {
                return null;
            }

            if (story.UserId == userId)
            {
                return new StoryViewer
                {
                    UserId = userId,
                    StoryId = storyId,
                };
            }

            var viewer = _context.StoriesViewer
                .SingleOrDefault(sv => sv.UserId == userId
                                    && sv.StoryId == storyId);
            if (viewer != null)
            {
                return viewer;
            }

            var newViewer = new StoryViewer
            {
                ViewedAt = DateTime.Now,
                UserId = userId,
                StoryId = storyId
            };

            _context.StoriesViewer.Add(newViewer);
            _context.SaveChanges();

            return newViewer;
        }

        public bool HasUserUploadedVisibleStoryForLoggedInUser(string storyOwnerId, string loggedInUserId)
        {
            var stories = GetStoriesOfUser(storyOwnerId, loggedInUserId);
            return stories.Any();
        }

        public IReadOnlyCollection<StoryViewModel> GetFirstStoriesOfFollowedUsers(string userId)
        {
            var followings = _friendsService.GetFollowing(userId);
            var userIds = followings.Select(f => f.Id).ToList();

            var friends = _friendsService.GetFriends(userId);
            var friendIds = friends.Select(f => f.Id).ToList();

            var stories = _context.Stories
                .Where(
                    s => userIds.Contains(s.UserId)
                    &&
                    (s.Visibility == Visibility.Public
                    || s.Visibility == Visibility.Followers
                    || (friendIds.Contains(s.UserId)
                       &&
                       (s.Visibility == Visibility.Friends
                       || s.Visibility == Visibility.Following))
                    )
                )
                .OrderBy(s => s.CreatedAt)
                .Select(s => new StoryViewModel
                {
                    Id = s.Id,
                    CreatedAt = s.CreatedAt,
                    ExpiresAt = s.ExpiresAt,
                    Visibility = s.Visibility,
                    UserId = s.UserId,
                    MediaContent = s.MediaContent,
                    ViewCount = s.StoryViewers.Count()
                })
                .ToList();

            return stories
               .GroupBy(s => s.UserId)
               .Select(g => g.First())
               .ToList();
        }
    }
}
