using CatchUp_server.Models.UserContent;

namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class UploadStoryViewModel
    {
        public string UserId { get; set; }
        public Visibility Visibility { get; set; }
        public IFormFile File { get; set; }
    }
}
