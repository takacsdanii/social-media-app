using CatchUp_server.Models.UserContent;

namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class StoryViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public Visibility Visibility { get; set; }
        public ICollection<string> Viewers { get; set; }
        public string UserId { get; set; }
        public int MediaContentId { get; set; }
    }
}
