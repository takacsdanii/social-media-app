using CatchUp_server.Interfaces;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserViewModel;
using CatchUp_server.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatchUp_server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Authorize]
        public IEnumerable<UserViewModel> List()
        {
            return _userService.List();
        }

        [HttpGet("user-by-id"), Authorize]
        public IActionResult GetUser(string id)
        {
            var user = _userService.GetUser(id);
            return (user != null) ? Ok(user) : NotFound();
        }

        [HttpDelete, Authorize]
        public IActionResult DeleteUser(string id)
        {
            var user = _userService.DeleteUser(id);
            return (user != null) ? Ok(user) : NotFound();
        }

        [HttpGet("user-by-email")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userService.GetUserByEmail(email);
            return (user != null) ? Ok(user) : NotFound();
        }

        [HttpPut, Authorize]
        public IActionResult UpdateUser([FromBody] UserViewModel userModel)
        {
            var result = _userService.UpdateUser(userModel);
            if(result.Succeeded)
            {
                return Ok(userModel);
            }
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { errors });
        }

        [HttpPut("update-gender"), Authorize]
        public IActionResult UpdateGender(string userId, Gender gender)
        {
            var _gender = _userService.UpdateGender(userId, gender);
            return (_gender != null) ? Ok(_gender) : NotFound();
        }

        [HttpGet("search-users"), Authorize]
        public IEnumerable<SearchUserViewModel> SearchUsers(string searchString)
        {
            return _userService.SearchUsers(searchString);
        }
    }
}
