using CatchUp_server.Db;
using CatchUp_server.Models.UserContent;
using CatchUp_server.ViewModels.UserContentViewModels;

namespace CatchUp_server.Services.UserContentServices
{
    public class StoryService
    {
        private readonly ApiDbContext _context;
        private readonly MediaFoldersService _mediaFoldersService;

        private const string storiesFolder = "Stories";

        public StoryService(ApiDbContext context, MediaFoldersService mediaFoldersService)
        {
            _context = context;
            _mediaFoldersService = mediaFoldersService;
        }

        // might need to modify mediafolderservice a bit
        public StoryViewModel UploadStory(UploadStoryViewModel uploadModel, IFormFile file)
        {
            var user = _context.Users.SingleOrDefault(u =>  u.Id == uploadModel.UserId);
            if(user == null)
            {
                return null;
            }

            var story = new Story
            {
                UserId = user.Id,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddHours(24),
                Visibility = uploadModel.Visibility,
                StoryViewers = new List<StoryViewer>(),
            };
        }
    }
}
