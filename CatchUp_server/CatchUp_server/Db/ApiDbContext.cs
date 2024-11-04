using CatchUp_server.Models.UserContent;
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
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryViewer> StoriesViewer { get; set; }
        public DbSet<MediaContent> MediaContents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var user = new IdentityRole("user");
            user.NormalizedName = "user";

            builder.Entity<IdentityRole>().HasData(admin, user);

            // Friendships
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

            // Users & Posts
            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.Userid)
                .OnDelete(DeleteBehavior.Cascade);

            // Users & Stories
            builder.Entity<Story>()
                .HasOne(s => s.User)
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Posts & MediaContents
            builder.Entity<MediaContent>()
                .HasOne(mc => mc.Post)
                .WithMany(p => p.MediaContents)
                .HasForeignKey(mc => mc.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            //// Stories & MediaContents
            //builder.Entity<MediaContent>()
            //    .HasOne(mc => mc.Story)
            //    .WithOne(s => s.MediaContent)
            //    .HasForeignKey<MediaContent>(mc => mc.StoryId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Posts & Comments
            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Post & Likes
            builder.Entity<Like>()
               .HasOne(l => l.Post)
               .WithMany(p => p.Likes)
               .HasForeignKey(l => l.PostId)
               .OnDelete(DeleteBehavior.Restrict);

            // MediaContents & Stories
            builder.Entity<Story>()
                .HasOne(s => s.MediaContent)
                .WithOne(/*mc => mc.Story*/)
                .HasForeignKey<Story>(s => s.MediaContentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Stories & Viewers
            builder.Entity<StoryViewer>()
                .HasOne(sv => sv.User)
                .WithMany()
                .HasForeignKey(sv => sv.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Stories & Viewers
            builder.Entity<StoryViewer>()
                .HasOne(sv => sv.Story)
                .WithMany(s => s.StoryViewers)
                .HasForeignKey(sv => sv.StoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comments & Replies
            builder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(pc => pc.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comments & Likes
            builder.Entity<Like>()
                .HasOne(l => l.Comment)
                .WithMany(c => c.Likes)
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Users & Likes
            builder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Users & Comments
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
