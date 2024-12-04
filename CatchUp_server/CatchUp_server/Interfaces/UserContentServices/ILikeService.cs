using CatchUp_server.ViewModels.UserContentViewModels;

namespace CatchUp_server.Interfaces.UserContentServices
{
    public interface ILikeService
    {
        int? LikePost(string userId, int postId);
        int? LikeComment(string userId, int postId, int commentId);
        int? RemoveLike(int likeId);
        IReadOnlyCollection<LikeViewModel> GetLikersForPost(int postId);
        IReadOnlyCollection<LikeViewModel> GetLikersForComment(int postId, int commentId);
        int GetLikeId(string userId, int postId, int? commentId);
    }
}
