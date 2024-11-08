using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatchUp_server.Models.UserContent
{
    public class MediaContent
    {
        public int Id { get; set; }
        public MediaType Type { get; set; }
        public string MediaUrl { get; set; }

        [JsonIgnore]
        public Post Post { get; set; }
        public int? PostId { get; set; }
    }
}
