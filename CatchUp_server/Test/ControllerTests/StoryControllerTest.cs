using CatchUp_server.Controllers;
using CatchUp_server.Controllers.UserContentControllers;
using CatchUp_server.Interfaces;
using CatchUp_server.Interfaces.UserContentServices;
using CatchUp_server.Models.UserContent;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserContentViewModels;
using CatchUp_server.ViewModels.UserViewModel;
using CatchUp_server.ViewModels.UserViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Test
{
    public class StoryControllerTest
    {
        private readonly StoryController _controller;
        private readonly IStoryService _fakeService;

        private User fakeUser;
        private IReadOnlyCollection<StoryViewModel> fakeStories;
        public StoryControllerTest()
        {
            _fakeService = A.Fake<IStoryService>();
            _controller = new StoryController(_fakeService);
            MockUser();
            MockStory();
        }

        private void MockUser()
        {
            fakeUser = new User
            {
                Id = "user1",
                UserName = "BilboBaggins",
                Email = "bilbobaggins@gmai.com",
                Gender = Gender.Male,
            };
        }

        private void MockStory()
        {
            fakeStories = new List<StoryViewModel>()
            {
                new StoryViewModel
                {
                    Id = 1,
                    UserId = fakeUser.Id,
                    Visibility = Visibility.Friends,
                },
                new StoryViewModel
                {
                    Id = 2,
                    UserId = fakeUser.Id,
                    Visibility = Visibility.Public,
                },
                new StoryViewModel
                {
                    Id = 3,
                    UserId = "id2",
                    Visibility = Visibility.Public,
                },
            };
        }

        [Fact]
        public void GetStoriesOfUser_ShouldReturnStoriesForGivenUserId()
        {
            // Arrange
            var expectedStories = fakeStories.Where(s => s.UserId == fakeUser.Id).ToList();
            A.CallTo(() => _fakeService.GetStoriesOfUser(fakeUser.Id))
                .Returns(expectedStories);

            // Act
            var result = _controller.GetStoriesOfUser(fakeUser.Id);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedStories);
            A.CallTo(() => _fakeService.GetStoriesOfUser(fakeUser.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetStoriesOfUser_ShouldReturnEmpty_WhenNoStoriesForUserId()
        {
            // Arrange
            var userId = "userNotFound";
            var expectedStories = new List<StoryViewModel>();
            A.CallTo(() => _fakeService.GetStoriesOfUser(userId))
                .Returns(expectedStories);

            // Act
            var result = _controller.GetStoriesOfUser(userId);

            // Assert
            result.Should().BeEmpty();
            A.CallTo(() => _fakeService.GetStoriesOfUser(userId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetVisibleStoriesOfUser_ShouldReturnStoriesBasedOnVisibility()
        {
            // Arrange
            var storyOwnerId = fakeUser.Id;
            var loggedInUserId = "id2";
            var expectedStories = fakeStories.Where(s => s.UserId == storyOwnerId &&
                                                          (s.Visibility == Visibility.Public ||
                                                           (s.Visibility == Visibility.Friends && s.UserId == loggedInUserId)))
                                             .ToList();

            A.CallTo(() => _fakeService.GetStoriesOfUser(storyOwnerId, loggedInUserId))
                .Returns(expectedStories);

            // Act
            var result = _controller.GetStoriesOfUser(storyOwnerId, loggedInUserId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedStories);
            A.CallTo(() => _fakeService.GetStoriesOfUser(storyOwnerId, loggedInUserId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetVisibleStoriesOfUser_ShouldReturnEmpty_WhenNoVisibleStories()
        {
            // Arrange
            var storyOwnerId = "user1";
            var loggedInUserId = "userNotFound";
            var expectedStories = new List<StoryViewModel>();

            A.CallTo(() => _fakeService.GetStoriesOfUser(storyOwnerId, loggedInUserId))
                .Returns(expectedStories);

            // Act
            var result = _controller.GetStoriesOfUser(storyOwnerId, loggedInUserId);

            // Assert
            result.Should().BeEmpty();
            A.CallTo(() => _fakeService.GetStoriesOfUser(storyOwnerId, loggedInUserId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetVisibleStoriesOfUser_ShouldOnlyReturnFriendsStories()
        {
            // Arrange
            var storyOwnerId = fakeUser.Id;
            var loggedInUserId = "id2";
            var expectedStories = fakeStories
                .Where(s => s.UserId == storyOwnerId && s.Visibility == Visibility.Friends)
                .ToList();

            A.CallTo(() => _fakeService.GetStoriesOfUser(storyOwnerId, loggedInUserId))
                .Returns(expectedStories);

            // Act
            var result = _controller.GetStoriesOfUser(storyOwnerId, loggedInUserId);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            result.Should().BeEquivalentTo(expectedStories);
            A.CallTo(() => _fakeService.GetStoriesOfUser(storyOwnerId, loggedInUserId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetStory_ShouldReturnStory_WhenStoryExists()
        {
            // Arrange
            var storyId = 1; 
            var expectedStory = fakeStories.First();
            A.CallTo(() => _fakeService.GetStory(storyId))
                .Returns(expectedStory);

            // Act
            var result = _controller.GetStory(storyId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expectedStory);
            A.CallTo(() => _fakeService.GetStory(storyId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetStory_ShouldReturnNotFound_WhenStoryDoesNotExist()
        {
            // Arrange
            var storyId = 1;
            StoryViewModel expectedStory = null;

            A.CallTo(() => _fakeService.GetStory(storyId))
                .Returns(expectedStory);

            // Act
            var result = _controller.GetStory(storyId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            A.CallTo(() => _fakeService.GetStory(storyId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void UploadStory_ShouldReturnStory_WhenUploadIsSuccessful()
        {
            // Arrange
            var uploadRequest = new UploadStoryViewModel
            {
                UserId = "user1",
                Visibility = Visibility.Public,
            };

            var expectedStory = new StoryViewModel
            {
                Id = 1,
                UserId = "user1",
                Visibility = Visibility.Public,
            };

            A.CallTo(() => _fakeService.UploadStory(uploadRequest))
                .Returns(expectedStory);

            // Act
            var result = _controller.UploadStory(uploadRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expectedStory);
            A.CallTo(() => _fakeService.UploadStory(uploadRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void UploadStory_ShouldReturnNotFound_WhenUploadFails()
        {
            // Arrange
            var uploadRequest = new UploadStoryViewModel
            {
                UserId = "user1",
                Visibility = Visibility.Public,
            };

            A.CallTo(() => _fakeService.UploadStory(uploadRequest))
                .Returns((StoryViewModel)null);

            // Act
            var result = _controller.UploadStory(uploadRequest);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            A.CallTo(() => _fakeService.UploadStory(uploadRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Delete_ShouldReturnOk_WhenDeletionIsSuccessful()
        {
            // Arrange
            var storyId = 1;

            A.CallTo(() => _fakeService.Delete(storyId))
                .Returns(true);

            // Act
            var result = _controller.Delete(storyId);

            // Assert
            result.Should().BeOfType<OkResult>();
            A.CallTo(() => _fakeService.Delete(storyId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenStoryDoesNotExist()
        {
            // Arrange
            var storyId = 1;
            A.CallTo(() => _fakeService.Delete(storyId))
                .Returns(false);

            // Act
            var result = _controller.Delete(storyId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            A.CallTo(() => _fakeService.Delete(storyId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetStoryViewers_ShouldReturnViewers_WhenViewersExist()
        {
            // Arrange
            var storyId = 1;
            var expectedViewers = new List<UserPreviewViewModel>
            {
                new UserPreviewViewModel { Id = "user1", UserName = "BilboBaggins" },
                new UserPreviewViewModel { Id = "user2", UserName = "FrodoBaggins" }
            };

            A.CallTo(() => _fakeService.GetStoryViewers(storyId))
                .Returns(expectedViewers);

            // Act
            var result = _controller.GetStoryViewers(storyId);

            // Assert
            result.Should().BeEquivalentTo(expectedViewers);
            A.CallTo(() => _fakeService.GetStoryViewers(storyId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetStoryViewers_ShouldReturnEmpty_WhenNoViewersExist()
        {
            // Arrange
            var storyId = 1;
            var expectedViewers = new List<UserPreviewViewModel>();

            A.CallTo(() => _fakeService.GetStoryViewers(storyId))
                .Returns(expectedViewers);

            // Act
            var result = _controller.GetStoryViewers(storyId);

            // Assert
            result.Should().BeEmpty();
            A.CallTo(() => _fakeService.GetStoryViewers(storyId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void AddViewerToStory_ShouldReturnViewer_WhenViewerAddedSuccessfully()
        {
            // Arrange
            var userId = "user1";
            var storyId = 1;
            var expectedViewer = new StoryViewer { UserId = userId, StoryId = storyId };

            A.CallTo(() => _fakeService.AddViewerToStory(userId, storyId))
                .Returns(expectedViewer);

            // Act
            var result = _controller.AddViewerToStory(userId, storyId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expectedViewer);
            A.CallTo(() => _fakeService.AddViewerToStory(userId, storyId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void AddViewerToStory_ShouldReturnNotFound_WhenViewerCannotBeAdded()
        {
            // Arrange
            var userId = "user1";
            var storyId = 1;

            A.CallTo(() => _fakeService.AddViewerToStory(userId, storyId))
                .Returns(null);

            // Act
            var result = _controller.AddViewerToStory(userId, storyId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            A.CallTo(() => _fakeService.AddViewerToStory(userId, storyId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void HasUserUploadedVisibleStoryForLoggedInUser_ShouldReturnTrue_WhenUserHasUploadedVisibleStory()
        {
            // Arrange
            var storyOwnerId = "user1";
            var loggedInUserId = "user2";
            var expectedResult = true;

            A.CallTo(() => _fakeService.HasUserUploadedVisibleStoryForLoggedInUser(storyOwnerId, loggedInUserId))
                .Returns(expectedResult);

            // Act
            var result = _controller.HasUserUploadedVisibleStoryForLoggedInUser(storyOwnerId, loggedInUserId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(new { result = expectedResult });
            A.CallTo(() => _fakeService.HasUserUploadedVisibleStoryForLoggedInUser(storyOwnerId, loggedInUserId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetFirstStoriesOfFollowedUsers_ShouldReturnStories_WhenValidUserIdIsProvided()
        {
            // Arrange
            var userId = "user1";
            var expectedStories = new List<StoryViewModel>
            {
                new StoryViewModel { Id = 1, UserId = "user2", Visibility = Visibility.Public },
                new StoryViewModel { Id = 2, UserId = "user3", Visibility = Visibility.Public }
            };

            A.CallTo(() => _fakeService.GetFirstStoriesOfFollowedUsers(userId))
                .Returns(expectedStories);

            // Act
            var result = _controller.GetFirstStoriesOfFollowedUsers(userId);

            // Assert
            result.Should().BeEquivalentTo(expectedStories);
            A.CallTo(() => _fakeService.GetFirstStoriesOfFollowedUsers(userId)).MustHaveHappenedOnceExactly();
        }

    }
}