using CatchUp_server.Models.UserModels;

namespace CatchUp_server.Models.UserContent
{
    public class Comment
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public UserProfile Profile { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        // ezen a kettőn még gondolkozok
        public List<Like> Like { get; set; }
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
