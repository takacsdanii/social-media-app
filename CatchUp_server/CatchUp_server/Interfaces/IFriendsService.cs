using CatchUp_server.ViewModels.UserViewModels;

namespace CatchUp_server.Interfaces
{
    public interface IFriendsService
    {
        IReadOnlyCollection<UserPreviewViewModel> GetFollowers(string userId);
        IReadOnlyCollection<UserPreviewViewModel> GetFollowing(string userId);
        IReadOnlyCollection<UserPreviewViewModel> GetFriends(string userId);
        bool? doesUserFollowTargetUser(string userId, string targetUserId);
        bool? FollowUser(string userId, string targetUserId);
        bool? UnFollowUser(string userId, string targetUserId);
    }
}
