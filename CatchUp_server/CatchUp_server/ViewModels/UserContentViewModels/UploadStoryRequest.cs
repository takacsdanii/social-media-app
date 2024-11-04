namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class UploadStoryRequest
    {
        public UploadStoryViewModel Story { get; set; }
        public IFormFile File { get; set; }
    }
}
