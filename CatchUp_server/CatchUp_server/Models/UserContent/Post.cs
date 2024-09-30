using CatchUp_server.Models.UserModels;

namespace CatchUp_server.Models.UserContent
{
    public class Post
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public UserProfile Profile { get; set; }

        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public List<string> MediaUrl { get; set; } // közös url videókhoz és képekhez

        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
        //public List<Share> Shares { get; set; }
    }
}
