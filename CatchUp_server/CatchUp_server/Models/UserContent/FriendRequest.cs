using CatchUp_server.Models.UserModels;

namespace CatchUp_server.Models.UserContent
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }

        public UserProfile Sender { get; set; }
        public UserProfile Reciever { get; set; }

        public DateTime SentAt { get; set; }
    }
}
