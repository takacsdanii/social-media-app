using CatchUp_server.Db;

namespace CatchUp_server.Services.UserContentServices
{
    public class UserContentService
    {
        private readonly ApiDbContext _context;

        public UserContentService(ApiDbContext context)
        {
            _context = context;
        }
    }
}
