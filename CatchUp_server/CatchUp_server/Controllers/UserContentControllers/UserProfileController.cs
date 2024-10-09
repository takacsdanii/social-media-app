using CatchUp_server.Models.UserContent;
using CatchUp_server.Services.UserContentServices;
using CatchUp_server.ViewModels.UserContentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CatchUp_server.Controllers.UserContentControllers
{
    [Route("api/user-content/user-profile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileService _userProfileService;

        public UserProfileController(UserProfileService userContentService)
        {
            _userProfileService = userContentService;
        }

        [HttpPut("profile-picture"), Authorize]
        public IActionResult EditProfilePic(string userId, IFormFile file)
        {
            var result = _userProfileService.EditProfilePic(userId, file);
            return result ? Ok() : NotFound();
        }

        [HttpPut("cover-picture"), Authorize]
        public IActionResult EditCoverPic(string userId, IFormFile file)
        {
            var result = _userProfileService.EditCoverPic(userId, file);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("profile-picture"), Authorize]
        public IActionResult DeleteProfilePic(string userId, string fileName)
        {
            var result = _userProfileService.DeleteProfilePic(userId, fileName);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("cover-picture"), Authorize]
        public IActionResult DeleteCoverPic(string userId, string fileName)
        {
            var result = _userProfileService.DeleteCoverPic(userId, fileName);
            return result ? Ok() : NotFound();
        }

        [HttpPut("bio"), Authorize]
        public IActionResult EditBio([FromBody] EditBioViewModel editBioViewModel)
        {
            var result = _userProfileService.EditBio(editBioViewModel);
            return result ? Ok(editBioViewModel) : NotFound();
        }

    }
}
