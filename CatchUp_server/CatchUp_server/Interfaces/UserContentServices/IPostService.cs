using CatchUp_server.Models.UserContent;
using CatchUp_server.ViewModels.UserContentViewModels;

namespace CatchUp_server.Interfaces.UserContentServices
{
    public interface IPostService
    {
        PostViewModel GetPost(int postId);
        IReadOnlyCollection<PostViewModel> GetPostsOfUser(string userId);
        IReadOnlyCollection<PostViewModel> GetPostsOfUser(string postOwnerId, string loggedInUserId);
        PostViewModel UploadPost(UploadPostViewModel postModel, List<IFormFile> files);
        string EditDescription(int postId, string? description);
        Visibility? EditVisibility(int postId, Visibility visibility);
        bool Delete(int postId);
        IReadOnlyCollection<PostViewModel> GetPostsOfFollowedUsers(string userId);
    }
}
