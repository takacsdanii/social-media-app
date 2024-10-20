using CatchUp_server.Models.UserContent;
using CatchUp_server.Models.UserModels;

namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Visibility Visibility { get; set; }
        public string UserId { get; set; }

        public ICollection<string> MediaUrls { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
