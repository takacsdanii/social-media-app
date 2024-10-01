namespace CatchUp_server.Models.UserModels
{
    public class FriendShip
    {
        public int Id { get; set; }

        public string FollowerUserId { get; set; }
        public User FollowerUser { get; set; }

        public string FollowedUserId { get; set; }
        public User FollowedUser { get; set; }
    }
}
