using CatchUp_server.Services.UserContentServices;
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

        [HttpPut("profile-picture")]
        public IActionResult EditProfilePic(string userId, IFormFile file)
        {
            var result = _userContentService.EditProfilePic(userId, file);
            return (result) ? Ok(): NotFound();
        }
    }
}
