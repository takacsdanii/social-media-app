using CatchUp_server.Models.UserContent;

namespace CatchUp_server.ViewModels.UserContentViewModels
{
    public class UploadPostViewModel
    {
        public string UserId { get; set; }
        public string? Description { get; set; }
        public Visibility Visibility { get; set; }
    }
}
