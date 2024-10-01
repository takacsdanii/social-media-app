using CatchUp_server.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserContent
{
    public class Story
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public ICollection<StoryViewer> StoryViewers { get; set; }

        public int MediaContentId { get; set; }
        public MediaContent MediaContent { get; set; }


        public int UserID { get; set; }
        public User User { get; set; }
    }
}
