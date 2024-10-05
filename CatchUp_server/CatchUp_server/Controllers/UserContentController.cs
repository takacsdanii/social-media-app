using CatchUp_server.Services.UserContentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatchUp_server.Controllers
{
    [Route("api/user-content")]
    [ApiController]
    public class UserContentController : ControllerBase
    {
        private readonly UserContentService _userContentService;

        public UserContentController(UserContentService userContentService)
        {
            _userContentService = userContentService;
        }

        [HttpPut("profile-picture"), Authorize]
        public IActionResult EditProfilePic(string userId, IFormFile file)
        {
            var result = _userContentService.EditProfilePic(userId, file);
            return (result) ? Ok(): NotFound();
        }

        [HttpPut("cover-picture"), Authorize]
        public IActionResult EditCoverPic(string userId, IFormFile file)
        {
            var result = _userContentService.EditCoverPic(userId, file);
            return (result) ? Ok() : NotFound();
        }

        [HttpDelete("profile-picture")]
        public IActionResult DeleteProfilePic(string userId, string fileName)
        {
            var result = _userContentService.DeleteProfilePic(userId, fileName);
            return (result) ? Ok() : NotFound();
        }

        [HttpDelete("cover-picture")]
        public IActionResult DeleteCoverPic(string userId, string fileName)
        {
            var result = _userContentService.DeleteCoverPic(userId, fileName);
            return (result) ? Ok() : NotFound();
        }
    }
}
