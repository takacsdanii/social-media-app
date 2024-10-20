namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePicUrl { get; set; }
        public ICollection<LikeViewModel> Likers { get; set; } = new List<LikeViewModel>();
        //public ICollection<CommentViewModel> Repies { get; set; } = new List<CommentViewModel>();
        public int LikeCount { get; set; }



        // IF THERE IS AN ERROR, THIS IS LIKELY TO CAUSE IT!!!

        //int PostId { get; set; }
        //int? CommentId { get; set; } 
    }
}
