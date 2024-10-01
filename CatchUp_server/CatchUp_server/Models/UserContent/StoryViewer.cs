using CatchUp_server.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserContent
{
    public class StoryViewer
    {
        public int Id { get; set; }
        public DateTime ViewedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int StoryId { get; set; }
        public Story Story { get; set; }
    }
}
