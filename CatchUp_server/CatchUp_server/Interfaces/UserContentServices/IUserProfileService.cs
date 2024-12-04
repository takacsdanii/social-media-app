using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserContentViewModels;

namespace CatchUp_server.Interfaces.UserContentServices
{
    public interface IUserProfileService
    {
        void setDefaultProfilePic(User user);
        void setDefaultCoverPic(User user);
        bool EditProfilePic(string userId, IFormFile file);
        bool EditCoverPic(string userId, IFormFile file);
        bool DeleteProfilePic(string userId, string fileName);
        bool DeleteCoverPic(string userId, string fileName);
        bool EditBio(EditBioViewModel editBioViewModel);
    }
}
