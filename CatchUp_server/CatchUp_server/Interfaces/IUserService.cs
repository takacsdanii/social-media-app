using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserViewModel;
using CatchUp_server.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;

namespace CatchUp_server.Interfaces
{
    public interface IUserService
    {
        IReadOnlyCollection<UserViewModel> List();
        User DeleteUser(string userId);
        UserViewModel GetUser(string id);
        UserViewModel GetUserByEmail(string email);
        IdentityResult UpdateUser(UserViewModel userModel);
        Gender? UpdateGender(string userId, Gender gender);
        IReadOnlyCollection<SearchUserViewModel> SearchUsers(string searchString);
    }
}
