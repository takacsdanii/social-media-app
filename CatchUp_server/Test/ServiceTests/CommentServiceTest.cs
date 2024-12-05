using CatchUp_server.Models.UserContent;
using CatchUp_server.Services.UserContentServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using CatchUp_server.Db;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserContentViewModels;

namespace CatchUp_Test.ServiceTests
{
    public class CommentServiceTest
    {
        private readonly CommentService _service;

        public CommentServiceTest()
        {
            _service = new CommentService(GetDbContext());
        }

        private ApiDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApiDbContext(options);
            context.Database.EnsureCreated();
            SeedDatabase(context);
            return context;
        }

        private void SeedDatabase(ApiDbContext context)
        {
            // Create some users
            context.Users.AddRange(new List<User>
            {
                new User { Id = "user1", UserName = "User One", CoverPicUrl = "", ProfilePicUrl = "", FirstName = "", LastName = "" },
                new User { Id = "user2", UserName = "User Two", CoverPicUrl = "", ProfilePicUrl = "", FirstName = "", LastName = "" }
            });

            // Create a post
            context.Posts.Add(new Post
            {
                Id = 1,
                Description = "Test Post",
                Userid = "user1",
                Comments = new List<Comment>()
            });

            context.Comments.Add(new Comment
            {
                Id=100,
                PostId = 1,
                UserId = "user2",
                Text = "comment text"
            });

            context.Comments.Add(new Comment
            {
                Id = 200,
                PostId = 1,
                ParentCommentId = 100,
                UserId = "user2",
                Text = "comment text"
            });

            context.SaveChanges();
        }

        [Fact]
        public void AddComment_ShouldAddComment_WhenValidDataIsProvided()
        {
            // Arrange
            var userId = "user1";
            var postId = 1;
            var commentText = "This is a test comment";

            // Act
            var commentId = _service.AddCommentToPost(userId, postId, commentText);

            // Assert
            commentId.Should().NotBeNull();
        }

        [Fact]
        public void AddComment_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "nonexistentUser";
            var postId = 1;
            var commentText = "This is a test comment";

            // Act
            var commentId = _service.AddCommentToPost(userId, postId, commentText);

            // Assert
            commentId.Should().BeNull(); 
        }

        [Fact]
        public void AddComment_ShouldReturnNull_WhenPostDoesNotExist()
        {
            // Arrange
            var userId = "user1";
            var postId = 999; 
            var commentText = "This is a test comment";

            // Act
            var commentId = _service.AddCommentToPost(userId, postId, commentText);

            // Assert
            commentId.Should().BeNull();
        }

        [Fact]
        public void AddComment_ShouldAddReply_WhenParentCommentIdIsProvided()
        {
            // Arrange
            var userId = "user1";
            var postId = 1;
            var parentCommentId = 100; 
            var replyText = "This is a reply to the parent comment";

            // Act
            var replyId = _service.AddReplyToComment(userId, postId, parentCommentId, replyText);

            // Assert
            replyId.Should().NotBeNull();
        }

        [Fact]
        public void DeleteComment_ShouldDeleteCommentAndReplies_WhenValidCommentIdIsProvided()
        {
            // Arrange
            var commentid = 100;

            // Act
            var replyId = _service.DeleteComment(commentid);

            // Assert
            replyId.Should().NotBeNull();
        }

        [Fact]
        public void GetCommentsForPost_ShouldReturnComments_WhenValidPostIdIsProvided()
        {
            // Arrange
            int postid = 1;

            // Act
            var result = _service.GetCommentsForPost(postid);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().BeOfType<List<CommentViewModel>>();
        }

        [Fact]
        public void GetCommentsForPost_ShouldReturnEmptyList_WhenValidPostIdIsProvided()
        {
            // Arrange
            int postid = 15;

            // Act
            var result = _service.GetCommentsForPost(postid);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetRepliesForComment_ShouldReturnComments_WhenValidPostIdIsProvided()
        {
            // Arrange
            int postid = 1;
            int parentCommentid = 100;

            // Act
            var result = _service.GetRepliesForComment(postid, parentCommentid);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().BeOfType<List<CommentViewModel>>();
        }

        [Fact]
        public void GGetRepliesForComment_ShouldReturnEmptyList()
        {
            // Arrange
            int postid = 15;
            int parentCommentid = 100;

            // Act
            var result = _service.GetRepliesForComment(postid, parentCommentid);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetComment_ShouldReturnComment_WhenValidIdIsProvided()
        {
            // Arrange
            int id = 100;

            // Act
            var result = _service.GetCommentById(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CommentViewModel>();
            result.Text.Should().Be("comment text");
            result.UserId.Should().Be("user2");
        }

        [Fact]
        public void GetComment_ShouldReturnNull_WhenIdIsNotValid()
        {
            // Arrange
            int id = 43;

            // Act
            var result = _service.GetCommentById(id);

            // Assert
            result.Should().BeNull();
        }
    }
}
