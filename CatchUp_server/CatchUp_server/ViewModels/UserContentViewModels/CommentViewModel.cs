namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class CommentViewModel
    {
        int Id { get; set; }
        string Text { get; set; }
        int? PostId { get; set; }
        int? CommentId { get; set; } 
    }
}
