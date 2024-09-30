using CatchUp_server.Models.UserModels;

namespace CatchUp_server.Models.UserContent
{
    public class StoryView
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserProfile Viewer { get; set; }
        public DateTime ViewedAt { get; set; }
    }
}
