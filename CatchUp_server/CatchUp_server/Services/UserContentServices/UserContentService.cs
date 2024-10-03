using CatchUp_server.Db;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserViewModel;

namespace CatchUp_server.Services.UserContentServices
{
    public class UserContentService
    {
        private readonly ApiDbContext _context;

        public UserContentService(ApiDbContext context)
        {
            _context = context;
        }

        public bool EditProfilePic(string userId, string? imgUrl)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            user.ProfilePicUrl = imgUrl;
            _context.SaveChanges();
            return true;
        }

        public bool EditCoverPic(string userId, string? imgUrl)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            user.CoverPicUrl = imgUrl;
            _context.SaveChanges();
            return true;
        }

        public void DeleteProfilePic()
        {

        }

        public void DeleteCoverPic()
        {

        }

        public void UploadPost()
        {

        }

        public void DeletePost()
        {

        }

        public void EditPost()
        {

        }

        public void UploadStory()
        {

        }

        public void DeleteStory()
        {

        }

        public void AddComment()
        {

        }

        public void DeleteComment()
        {

        }

        public void EditComment()
        {

        }

        public void AddLike()
        {

        }

        public void RemoveLike()
        {

        }
    }
}
