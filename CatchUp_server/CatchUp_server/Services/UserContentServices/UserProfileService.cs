using CatchUp_server.Db;
using CatchUp_server.Interfaces.UserContentServices;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserContentViewModels;

namespace CatchUp_server.Services.UserContentServices
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApiDbContext _context;
        private readonly IMediaFoldersService _mediaFoldersService;

        private const string profilePicFolder = "ProfilePictures";
        private const string coverPicFolder = "CoverPictures";

        private const string defaultsFolder = "defaults";
        private const string defaultMaleProfilePic = "male.png";
        private const string defaultFemaleProfilePic = "female.png";
        private const string defaultOtherProfilePic = "other.jpg";
        private const string defaultCoverPic = "default-cover.jpg";

        public UserProfileService(ApiDbContext context, IMediaFoldersService mediaFoldersService)
        {
            _context = context;
            _mediaFoldersService = mediaFoldersService;
        }

        public void setDefaultProfilePic(User user)
        {
            var baseProfilePicUrl = $"/{defaultsFolder}/{profilePicFolder}";
            switch (user.Gender)
            {
                case Gender.Male:
                    user.ProfilePicUrl = $"{baseProfilePicUrl}/{defaultMaleProfilePic}";
                    break;

                case Gender.Female:
                    user.ProfilePicUrl = $"{baseProfilePicUrl}/{defaultFemaleProfilePic}";
                    break;

                case Gender.Other:
                    user.ProfilePicUrl = $"{baseProfilePicUrl}/{defaultOtherProfilePic}";
                    break;
            }
        }

        public void setDefaultCoverPic(User user)
        {
            user.CoverPicUrl = $"/{defaultsFolder}/{defaultCoverPic}";
        }

        private bool EditPic(string userId, IFormFile file, string folder)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            _mediaFoldersService.DeleteFolderContent(userId, folder);
            var imgUrl = _mediaFoldersService.UploadFile(userId, folder, file);

            if (folder == profilePicFolder)
            {
                user.ProfilePicUrl = imgUrl;
            }
            else if (folder == coverPicFolder)
            {
                user.CoverPicUrl = imgUrl;
            }
            else
            { 
                return false;
            }

            _context.SaveChanges();
            return true;
        }

        public bool EditProfilePic(string userId, IFormFile file)
        {
            return EditPic(userId, file, profilePicFolder);
        }

        public bool EditCoverPic(string userId, IFormFile file)
        {
            return EditPic(userId, file, coverPicFolder);
        }

        private bool DeletePic(string userId, string fileName, string folder)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            var result = _mediaFoldersService.DeleteFile(userId, folder, fileName);
            if (result == false)
            {
                return false;
            }

            if(folder == profilePicFolder)
            {
                setDefaultProfilePic(user);
            }
            else if(folder == coverPicFolder)
            {
                setDefaultCoverPic(user);
            } 
            
            _context.SaveChanges();
            return true;
        }

        public bool DeleteProfilePic(string userId, string fileName)
        {
            return DeletePic(userId, fileName, profilePicFolder);
        }

        public bool DeleteCoverPic(string userId, string fileName)
        {
            return DeletePic(userId, fileName, coverPicFolder);
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
