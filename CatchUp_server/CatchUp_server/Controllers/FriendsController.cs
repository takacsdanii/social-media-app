using CatchUp_server.Services.FriendsServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatchUp_server.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly FriendsService _friendsService;

        public FriendsController(FriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        [HttpGet("followers")]
        public IActionResult GetFollowers(string userId)
        {
            var follower = _friendsService.GetFollowers(userId);
            return (follower != null) ? Ok(follower) : NotFound();
        }

        [HttpGet("following")]
        public IActionResult GetFollowing(string userId)
        {
            var follower = _friendsService.GetFollowing(userId);
            return (follower != null) ? Ok(follower) : NotFound();
        }

        [HttpGet("friends")]
        public IActionResult GetFriends(string userId)
        {
            var follower = _friendsService.GetFriends(userId);
            return (follower != null) ? Ok(follower) : NotFound();
        }

        [HttpPost]
        public IActionResult FollowUser(string userId, string targetUserId)
        {
            var result = _friendsService.FollowUser(userId, targetUserId);
            if (result == true) return Ok();
            if (result == false) return BadRequest("Already following the user");
            return NotFound();
        }

        [HttpDelete]
        public IActionResult UnFollowUser(string userId, string targetUserId)
        {
            var result = _friendsService.UnFollowUser(userId, targetUserId);
            if (result == true) return Ok();
            if (result == false) return BadRequest("Didn't follow that user");
            return NotFound();
        }
    }
}
