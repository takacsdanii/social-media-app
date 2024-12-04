using CatchUp_server.Models.UserModels;

namespace CatchUp_server.ViewModels.UserViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }

        public string? Bio { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? CoverPicUrl { get; set; }
        public DateTime RegisteredAt { get; set; }

        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }
}
