using CatchUp_server.Models.UserContent;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserModels
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Bio { get; set; }
        public string ProfilePicUrl { get; set; }
        public string CoverPicUrl { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Story> Stories { get; set; }
        public ICollection<UserProfile> Friends { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
