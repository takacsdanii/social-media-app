using CatchUp_server.Models.UserContent;
using CatchUp_server.ViewModels.UserViewModels;

namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class StoryViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public Visibility Visibility { get; set; }

        public string UserId { get; set; }
        public string MediaUrl { get; set; }
        public int ViewCount { get; set; }
    }
}
