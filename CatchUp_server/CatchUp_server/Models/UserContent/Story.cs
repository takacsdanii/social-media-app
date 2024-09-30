using CatchUp_server.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserContent
{
    public class Story
    {
        public int Id { get; set; }
        public string MediaUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DestroysAt { get; set; }

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // LEAVE THIS HERE FOR NOW, WILL DECIDE LATER
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //public ICollection<User> ViewedBy { get; set; }

        [ForeignKey("Profile")]
        public int ProfileId { get; set; }
        public UserProfile Profile { get; set; }

        public Story()
        {
            CreatedAt = DateTime.Now;
            DestroysAt = CreatedAt.AddHours(24);
        }
    }
}
