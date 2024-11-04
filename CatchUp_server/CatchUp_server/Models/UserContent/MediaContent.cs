using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserContent
{
    public class MediaContent
    {
        public int Id { get; set; }
        public MediaType Type { get; set; }
        public string MediaUrl { get; set; }

        public int? PostId { get; set; }
        public Post Post { get; set; }
    }
}
