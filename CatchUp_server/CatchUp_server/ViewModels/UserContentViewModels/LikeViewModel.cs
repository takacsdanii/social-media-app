namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class LikeViewModel
    {
        public int Id { get; set; }
        public DateTime LikedAt { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePicUrl { get; set; }
    }
}
