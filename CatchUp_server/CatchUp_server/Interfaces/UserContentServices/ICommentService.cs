using CatchUp_server.ViewModels.UserContentViewModels;

namespace CatchUp_server.Interfaces.UserContentServices
{
    public interface ICommentService
    {
        int? AddCommentToPost(string userId, int postId, string text);
        int? AddReplyToComment(string userId, int postId, int parentCommentId, string text);
        int? DeleteComment(int commentId);
        IReadOnlyCollection<CommentViewModel> GetCommentsForPost(int postId);
        IReadOnlyCollection<CommentViewModel> GetRepliesForComment(int postId, int parentCommentId);
        CommentViewModel GetCommentById(int id);
    }
}
