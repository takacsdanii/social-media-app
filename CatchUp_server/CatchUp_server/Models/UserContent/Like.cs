using CatchUp_server.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserContent
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime LikedAt { get; set; }

        [ForeignKey("Profile")]
        public int ProfileId { get; set; }
        public UserProfile Profile { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Comment")]
        public int? CommentId { get; set; }
        public Comment Comment { get; set; }        
    }
}
