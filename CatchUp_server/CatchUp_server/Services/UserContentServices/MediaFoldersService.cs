using CatchUp_server.Db;

namespace CatchUp_server.Services.UserContentServices
{
    public class MediaFoldersService
    {
        public ApiDbContext _context;
        public IWebHostEnvironment _environment;
        public MediaFoldersService(ApiDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment; 
        }

        private string DoUserMediaFoldersExist(string userId)
        {
            string userRootPath = Path.Combine(_environment.WebRootPath, "UserMedia", userId);
            if (Directory.Exists(userRootPath))
            {
                return userRootPath;
            }
            return null;
        }

        private void CreateUserMediaFolders(string userId)
        {
            var userRootPath = DoUserMediaFoldersExist(userId);
            if (userRootPath == null)
            {
                string rootPath = Path.Combine(_environment.WebRootPath, "UserMedia", userId);
                Directory.CreateDirectory(Path.Combine(rootPath, "ProfilePictures"));
                Directory.CreateDirectory(Path.Combine(rootPath, "CoverPictures"));
                Directory.CreateDirectory(Path.Combine(rootPath, "Posts"));
            }
        }

        public void DeleteUserMediaFolders(string userId)
        {
            var userRootPath = DoUserMediaFoldersExist(userId);
            if (userRootPath != null)
            {
                Directory.Delete(userRootPath, true);
            }
        }

        public string UploadFile(string userId, string folder, IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                return null;
            }

            string userPath = Path.Combine(_environment.WebRootPath, "UserMedia", userId, folder);
            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(userPath, uniqueFileName);

            CreateUserMediaFolders(userId);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            string fileUrl = $"/UserMedia/{userId}/{folder}/{uniqueFileName}";
            return fileUrl;
        }

        public bool DeleteFile(string userId, string folder, string fileName)
        {
            string userPath = Path.Combine(_environment.WebRootPath, "UserMedia", userId, folder);
            string filePath = Path.Combine(userPath, fileName);

            if(!File.Exists(filePath))
            {
                return false;
            }

            File.Delete(filePath);
            return true;
        }
    }
}
