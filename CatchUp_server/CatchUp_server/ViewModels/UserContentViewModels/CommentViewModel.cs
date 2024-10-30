namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePicUrl { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }
    }
}
