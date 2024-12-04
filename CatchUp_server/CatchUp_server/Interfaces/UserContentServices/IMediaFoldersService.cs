using CatchUp_server.Models.UserContent;

namespace CatchUp_server.Interfaces.UserContentServices
{
    public interface IMediaFoldersService
    {
        void DeleteUserMediaFolders(string userId);
        string UploadFile(string userId, string folder, IFormFile file);
        bool DeleteFile(string userId, string folder, string fileName);
        void DeleteFolderContent(string userId, string folder);
        MediaType GetMediaType(IFormFile file);
    }
}
