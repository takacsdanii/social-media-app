using CatchUp_server.Models.UserContent;
using CatchUp_server.Services.UserContentServices;
using CatchUp_server.ViewModels.UserContentViewModels;
using CatchUp_server.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatchUp_server.Controllers.UserContentControllers
{
    [Route("api/user-content/story")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly StoryService _storyService;

        public StoryController(StoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpGet("get-stories-of-user"), Authorize]
        public IEnumerable<StoryViewModel> GetStoriesOfUser(string userId)
        {
            return _storyService.GetStoriesOfUser(userId);
        }

        [HttpGet("get-visible-stories-of-user"), Authorize]
        public IEnumerable<StoryViewModel> GetStoriesOfUser(string storyOwnerId, string loggedInUserId)
        {
            return _storyService.GetStoriesOfUser(storyOwnerId, loggedInUserId);
        }

        [HttpGet("story-by-id"), Authorize]
        public IActionResult GetStory(int storyId)
        {
            var story = _storyService.GetStory(storyId);
            return (story != null) ? Ok(story) : NotFound();
        }

        [HttpPost, Authorize]
        public IActionResult UploadStory([FromForm] UploadStoryViewModel request)
        {
            var story = _storyService.UploadStory(request);
            return (story != null) ? Ok(story) : NotFound();
        }

        [HttpDelete, Authorize]
        public IActionResult Delete(int storyId)
        {
            var result = _storyService.Delete(storyId);
            return (result) ? Ok() : NotFound();
        }

        [HttpGet("viewers"), Authorize]
        public IEnumerable<UserPreviewViewModel> GetStoryViewers(int storyId)
        {
            return _storyService.GetStoryViewers(storyId);
        }

        [HttpPost("add-viewer-to-story"), Authorize]
        public IActionResult AddViewerToStory(string userId, int storyId)
        {
            var viewer = _storyService.AddViewerToStory(userId, storyId);
            return (viewer != null) ? Ok(viewer) : NotFound();
        }

        [HttpGet("has-user-uploaded-story"), Authorize]
        public IActionResult HasUserUploadedVisibleStoryForLoggedInUser(string storyOwnerId, string loggedInUserId)
        {
            var result = _storyService.HasUserUploadedVisibleStoryForLoggedInUser(storyOwnerId, loggedInUserId);
            return Ok(new { result });
        }

        [HttpGet("first-stories-of-followed-users"), Authorize]
        public IEnumerable<StoryViewModel> GetFirstStoriesOfFollowedUsers(string userId)
        {
            return _storyService.GetFirstStoriesOfFollowedUsers(userId);
        }
    }
}
