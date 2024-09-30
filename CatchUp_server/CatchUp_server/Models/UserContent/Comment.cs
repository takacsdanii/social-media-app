using CatchUp_server.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserContent
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Replies { get; set; }

        [ForeignKey("Profile")]
        public int ProfileId { get; set; }
        public UserProfile Profile { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("ParentComment")]
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
    }
}
