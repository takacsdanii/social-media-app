using CatchUp_server.Models.UserContent;
using CatchUp_server.Services.UserContentServices;
using CatchUp_server.ViewModels.UserContentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        [HttpDelete("profile-picture"), Authorize]
        public IActionResult DeleteProfilePic(string userId, string fileName)
        {
            var result = _userContentService.DeleteProfilePic(userId, fileName);
            return (result) ? Ok() : NotFound();
        }

        [HttpDelete("cover-picture"), Authorize]
        public IActionResult DeleteCoverPic(string userId, string fileName)
        {
            var result = _userContentService.DeleteCoverPic(userId, fileName);
            return (result) ? Ok() : NotFound();
        }

        [HttpPut("bio"), Authorize]
        public IActionResult EditBio([FromBody] EditBioViewModel editBioViewModel)
        {
            var result = _userContentService.EditBio(editBioViewModel);
            return (result) ? Ok(editBioViewModel) : NotFound();
        }

        [HttpPost("post")]
        //public IActionResult UploadPost([FromBody] UploadPostViewModel postModel, [FromForm] List<IFormFile> files)
        public IActionResult UploadPost(string userId, string description, Visibility visibility, List<IFormFile> files)
        {
            //List<IFormFile> files = new List<IFormFile>();
            UploadPostViewModel postModel = new UploadPostViewModel()
            {
                UserId = userId,
                Description = description,
                Visibility = visibility
            };
            var post = _userContentService.UploadPost(postModel, files);
            return (post != null) ? Ok(post) : NotFound();
        }

        [HttpGet("posts-of-user")]
        public IActionResult ListPostsOfUser(string userId)
        {
            var posts = _userContentService.ListPostsOfUser(userId);
            return (posts != null) ? Ok(posts) : NotFound();
        }
    }
}
