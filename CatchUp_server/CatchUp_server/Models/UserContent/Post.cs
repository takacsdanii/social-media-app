using CatchUp_server.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserContent
{
    public class Post
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public ICollection<string> MediaUrls { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }

        [ForeignKey("Profile")]
        public int ProfileId { get; set; }
        public UserProfile Profile { get; set; }
    }
}
