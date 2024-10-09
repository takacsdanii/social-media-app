using CatchUp_server.Db;
using CatchUp_server.Models.UserContent;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserContentViewModels;
using CatchUp_server.ViewModels.UserViewModel;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel.Design;
using static System.Net.Mime.MediaTypeNames;

namespace CatchUp_server.Services.UserContentServices
{
    public class UserProfileService
    {
        private readonly ApiDbContext _context;
        private readonly MediaFoldersService _mediaFoldersService;

        public UserProfileService(ApiDbContext context, MediaFoldersService mediaFoldersService)
        {
            _context = context;
            _mediaFoldersService = mediaFoldersService;
        }

        public void setDefaultProfilePic(User user)
        {
            var baseProfilePicUrl = "/defaults/ProfilePictures";
            switch (user.Gender)
            {
                case Gender.Male:
                    user.ProfilePicUrl = $"{baseProfilePicUrl}/male.png";
                    break;

                case Gender.Female:
                    user.ProfilePicUrl = $"{baseProfilePicUrl}/female.png";
                    break;

                case Gender.Other:
                    user.ProfilePicUrl = $"{baseProfilePicUrl}/other.jpg";
                    break;
            }
        }

        public void setDefaultCoverPic(User user)
        {
            user.CoverPicUrl = "/defaults/default-cover.jpg";
        }

        public bool EditProfilePic(string userId, IFormFile file)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            // delete the original or leave it?
            var imgUrl = _mediaFoldersService.UploadFile(userId, "ProfilePictures", file);

            user.ProfilePicUrl = imgUrl;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteProfilePic(string userId, string fileName)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            var result = _mediaFoldersService.DeleteFile(userId, "ProfilePictures", fileName);
            if(result == false)
            {
                return false;
            }

            setDefaultProfilePic(user);
            _context.SaveChanges();
            return true;
        }

        public bool EditCoverPic(string userId, IFormFile file)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            // delete the original or leave it?
            var imgUrl = _mediaFoldersService.UploadFile(userId, "CoverPictures", file);

            user.CoverPicUrl = imgUrl;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteCoverPic(string userId, string fileName)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            var result = _mediaFoldersService.DeleteFile(userId, "CoverPictures", fileName);
            if (result == false)
            {
                return false;
            }

            setDefaultCoverPic(user);
            _context.SaveChanges();
            return true;
        }

        public bool EditBio(EditBioViewModel editBioViewModel)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == editBioViewModel.userId);
            if (user == null)
            {
                return false;
            }

            user.Bio = editBioViewModel.Bio;
            _context.SaveChanges();
            return true;
        }
    }
}
