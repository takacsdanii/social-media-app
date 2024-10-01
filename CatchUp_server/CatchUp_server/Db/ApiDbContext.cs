using CatchUp_server.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatchUp_server.Db
{
    public class ApiDbContext : IdentityDbContext<User>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<FriendShip> FriendShips { get; set; }
        //public DbSet<Post> Posts { get; set; }
        //public DbSet<Like> Likes { get; set; }
        //public DbSet<Comment> Comments { get; set; }
        //public DbSet<Story> Stories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var user = new IdentityRole("user");
            user.NormalizedName = "user";

            builder.Entity<IdentityRole>().HasData(admin, user);

            builder.Entity<FriendShip>()
                .HasOne(fs => fs.FollowerUser)
                .WithMany()
                .HasForeignKey(fs => fs.FollowerUserId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<FriendShip>()
                .HasOne(fs => fs.FollowedUser)
                .WithMany()
                .HasForeignKey(fs => fs.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
