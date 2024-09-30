using CatchUp_server.Models.UserModels;

namespace CatchUp_server.Models.UserContent
{
    public class Like
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public UserProfile Profile { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        // ezeken még gondolkozok
        public DateTime LikedAt { get; set; }

        public int? CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
