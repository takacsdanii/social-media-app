using CatchUp_server.Db;

namespace CatchUp_server.Services.UserContentServices
{
    public class MediaFoldersService
    {
        public ApiDbContext _context;
        public IWebHostEnvironment _environment;

        private const string mediaFolder = "UserMedia";
        private const string profilePicFolder = "ProfilePictures";
        private const string coverPicFolder = "CoverPictures";
        private const string postsFolder = "Posts";
        private const string storiesFolder = "Stories";

        public MediaFoldersService(ApiDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment; 
        }

        private string DoUserMediaFoldersExist(string userId)
        {
            string userRootPath = Path.Combine(_environment.WebRootPath, mediaFolder, userId);
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
                string rootPath = Path.Combine(_environment.WebRootPath, mediaFolder, userId);
                Directory.CreateDirectory(Path.Combine(rootPath, profilePicFolder));
                Directory.CreateDirectory(Path.Combine(rootPath, coverPicFolder));
                Directory.CreateDirectory(Path.Combine(rootPath, postsFolder));
                Directory.CreateDirectory(Path.Combine(rootPath, storiesFolder));
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

            string userPath = Path.Combine(_environment.WebRootPath, mediaFolder, userId, folder);
            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(userPath, uniqueFileName);

            CreateUserMediaFolders(userId);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            string fileUrl = $"/{mediaFolder}/{userId}/{folder}/{uniqueFileName}";
            return fileUrl;
        }

        public bool DeleteFile(string userId, string folder, string fileName)
        {
            string userPath = Path.Combine(_environment.WebRootPath, mediaFolder, userId, folder);
            string filePath = Path.Combine(userPath, fileName);

            if(!File.Exists(filePath))
            {
                return false;
            }

            File.Delete(filePath);
            return true;
        }

        public void DeleteFolderContent(string userId, string folder)
        {
            string path = Path.Combine(_environment.WebRootPath, mediaFolder, userId, folder) ;
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
        }
    }
}
