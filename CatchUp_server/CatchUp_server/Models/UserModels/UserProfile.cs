using CatchUp_server.Models.UserContent;

namespace CatchUp_server.Models.UserModels
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public string Bio { get; set; }
        public string ProfilePicUrl { get; set; }
        public string CoverPicUrl { get; set; }

        public List<Post> Posts { get; set; }
        public List<Story> Stories { get; set; }
        public List<UserProfile> Friends { get; set; }
        public List<FriendRequest> RecievedFriendRequests { get; set; }
        public List<FriendRequest> SentFriendRequests { get; set; }

        //public List<Chat> Chats { get; set; }

    }
}
