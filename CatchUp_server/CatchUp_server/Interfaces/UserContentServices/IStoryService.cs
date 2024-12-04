using CatchUp_server.Models.UserContent;
using CatchUp_server.ViewModels.UserContentViewModels;
using CatchUp_server.ViewModels.UserViewModels;

namespace CatchUp_server.Interfaces.UserContentServices
{
    public interface IStoryService
    {
        StoryViewModel GetStory(int id);
        IReadOnlyCollection<StoryViewModel> GetStoriesOfUser(string userId);
        IReadOnlyCollection<StoryViewModel> GetStoriesOfUser(string storyOwnerId, string loggedInUserId);
        StoryViewModel UploadStory(UploadStoryViewModel uploadModel);
        bool Delete(int storyId);
        IReadOnlyCollection<UserPreviewViewModel> GetStoryViewers(int id);
        StoryViewer AddViewerToStory(string userId, int storyId);
        bool HasUserUploadedVisibleStoryForLoggedInUser(string storyOwnerId, string loggedInUserId);
        IReadOnlyCollection<StoryViewModel> GetFirstStoriesOfFollowedUsers(string userId);
    }
}
