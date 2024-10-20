using CatchUp_server.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserContent
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime LikedAt { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int? CommentId { get; set; }
        public Comment Comment { get; set; }        
    }
}
