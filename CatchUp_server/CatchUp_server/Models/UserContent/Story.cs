using CatchUp_server.Models.UserModels;

namespace CatchUp_server.Models.UserContent
{
    public class Story
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public UserProfile Profile { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime DestroysAt { get; set; }
        public List<string> MediaUrl { get; set; } // közös url videókhoz és képekhez
        public List<StoryView> Views { get; set; }

        public Story()
        {
            CreatedAt = DateTime.Now;
            DestroysAt = CreatedAt.AddHours(24);
        }
    }
}
