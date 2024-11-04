using CatchUp_server.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatchUp_server.Models.UserContent
{
    public class StoryViewer
    {
        public int Id { get; set; }
        public DateTime ViewedAt { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public Story Story { get; set; }
        public int StoryId { get; set; }
    }
}
