using CatchUp_server.Models.UserModels;
using CatchUp_server.Models.UserContent;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CatchUp_server.Db
{
    public class ApiDbContext : IdentityDbContext<User>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Story> Stories { get; set; }
        //public DbSet<FriendRequest> FriendRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var user = new IdentityRole("user");
            user.NormalizedName = "user";

            builder.Entity<IdentityRole>().HasData(admin, user);
        }
    }
}
