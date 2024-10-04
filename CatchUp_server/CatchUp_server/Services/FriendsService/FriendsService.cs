using CatchUp_server.Db;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserViewModel;
using CatchUp_server.ViewModels.UserViewModels;

namespace CatchUp_server.Services.FriendsServices
{
    public class FriendsService
    {
        private readonly ApiDbContext _context;

        public FriendsService(ApiDbContext context)
        {
            _context = context;
        }

        public IReadOnlyCollection<DisplayUserModel> GetFollowers(string userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            var followers = _context.FriendShips
                .Where(f => f.FollowedUserId == userId)
                .Select(f => new DisplayUserModel
                {
                    Id = f.FollowerUserId,
                    UserName = f.FollowerUser.UserName,
                    ProfilePicUrl = f.FollowerUser.ProfilePicUrl
                })
                .ToList();

            return followers;
        }

        public IReadOnlyCollection<DisplayUserModel> GetFollowing(string userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            var following = _context.FriendShips
                .Where(f => f.FollowerUserId == userId)
                .Select(f => new DisplayUserModel
                {
                    Id= f.FollowedUserId,
                    UserName = f.FollowedUser.UserName,
                    ProfilePicUrl = f.FollowedUser.ProfilePicUrl
                })
                .ToList();

            return following;
        }

        public IReadOnlyCollection<DisplayUserModel> GetFriends(string userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            var followers = GetFollowers(userId);
            var following = GetFollowing(userId);

            var friends = following
                .Where(f => followers.Any(u => u.UserName == f.UserName))
                .ToList();

            return friends;
        }

        public bool? doesUserFollowTargetUser(string userId, string targetUserId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }
            var targetUser = _context.Users.SingleOrDefault(u => u.Id == targetUserId);
            if (targetUser == null)
            {
                return null;
            }

            var result = _context.FriendShips
                .SingleOrDefault(f => f.FollowerUserId == userId && f.FollowedUserId == targetUserId);

            return (result != null) ? true : false;
        }

        public bool? FollowUser(string userId, string targetUserId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            var targetUser = _context.Users.SingleOrDefault(u => u.Id == targetUserId);
            if (user == null || targetUser == null)
            {
                return null;
            }

            var alreadyFollowing = _context.FriendShips
                .Any(f => f.FollowerUserId == userId && f.FollowedUserId == targetUserId);

            if (alreadyFollowing)
            {
                return false;
            }

            var newFriendShip = new FriendShip
            {
                FollowerUserId = userId,
                FollowedUserId = targetUserId
            };

            _context.FriendShips.Add(newFriendShip);
            _context.SaveChanges();
            return true;
        }

        public bool? UnFollowUser(string userId, string targetUserId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            var targetUser = _context.Users.SingleOrDefault(u => u.Id == targetUserId);
            if (user == null || targetUser == null)
            {
                return null;
            }

            var friendShip = _context.FriendShips
                .SingleOrDefault(f => f.FollowerUserId == userId && f.FollowedUserId == targetUserId);

            if (friendShip == null)
            {
                return false;
            }

            _context.FriendShips.Remove(friendShip);
            _context.SaveChanges();
            return true;
        }
    }
}
