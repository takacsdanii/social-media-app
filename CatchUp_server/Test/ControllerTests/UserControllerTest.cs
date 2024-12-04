using CatchUp_server.Controllers;
using FakeItEasy;
using CatchUp_server.Interfaces;
using CatchUp_server.ViewModels.UserViewModel;
using FluentAssertions;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatchUp_Test.ControllerTests
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly IUserService _fakeService;

        private IReadOnlyCollection<UserViewModel> fakeUsers;
        private IReadOnlyCollection<UserViewModel> emptyList;
        private IReadOnlyCollection<UserViewModel> nullList;
        private UserViewModel fakeUser;

        public UserControllerTest()
        {
            _fakeService = A.Fake<IUserService>();
            _controller = new UserController(_fakeService);

            MockUser();
            MockUsers();
            emptyList = new List<UserViewModel>();
            nullList = null;        }

        private void MockUsers()
        {
            fakeUsers = new List<UserViewModel>
            {
                new UserViewModel { Id = "a1", UserName = "Gandalf" },
                new UserViewModel { Id = "a2", UserName = "PeterParker" },
                new UserViewModel { Id = "b1", UserName = "Voldemort" },
                new UserViewModel { Id = "b2", UserName = "GeraltOfRivia" },
                new UserViewModel { Id = "c1", UserName = "JohnCena" },
                new UserViewModel { Id = "c2", UserName = "BabyYoda" },
            };
        }

        private void MockUser()
        {
            fakeUser = new UserViewModel { Id = "user1", UserName = "BilboBaggins" };
        }

        [Fact]
        public void List_ShouldReturnAll_WhenListIsNotEmpty()
        {
            // Arrange
            A.CallTo(() => _fakeService.List())
                .Returns(fakeUsers);

            // Act
            var result = _controller.List();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(6);
            result.Should().BeEquivalentTo(fakeUsers);
            A.CallTo(() => _fakeService.List()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void List_ShouldReturnEmptyList_WhenListIsEmpty()
        {
            // Arrange
            A.CallTo(() => _fakeService.List())
                .Returns(emptyList);

            // Act
            var result = _controller.List();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            A.CallTo(() => _fakeService.List()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void List_ShouldReturnNull_WhenListIsNull()
        {
            // Arrange
            A.CallTo(() => _fakeService.List())
                .Returns(nullList);

            // Act
            var result = _controller.List();

            // Assert
            result.Should().BeNull();
            A.CallTo(() => _fakeService.List()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetUser_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            A.CallTo(() => _fakeService.GetUser(fakeUser.Id))
                .Returns(fakeUser);

            // Act
            var result = _controller.GetUser(fakeUser.Id);

            // Arrange
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeEquivalentTo(fakeUser);
            A.CallTo(() => _fakeService.GetUser(fakeUser.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetUser_ShouldReturnNotFound_WhenUserDoesntExist()
        {
            // Arrange
            var userId = "nonexistent-id";
            A.CallTo(() => _fakeService.GetUser(userId))
                .Returns(null);

            // Act
            var result = _controller.GetUser(userId);

            // Arrange
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
            A.CallTo(() => _fakeService.GetUser(userId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void DeleteUser_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = "userId1" };
            A.CallTo(() => _fakeService.DeleteUser(user.Id))
                .Returns(user);

            // Act
            var result = _controller.DeleteUser(user.Id);

            // Arrange
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeEquivalentTo(user);
            A.CallTo(() => _fakeService.DeleteUser(user.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void DeleteUser_ShouldReturnNotFound_WhenUserDoesntExist()
        {
            // Arrange
            var userId = "nonexistent-id";
            A.CallTo(() => _fakeService.DeleteUser(userId))
                .Returns(null);

            // Act
            var result = _controller.DeleteUser(userId);

            // Arrange
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
            A.CallTo(() => _fakeService.DeleteUser(userId)).MustHaveHappenedOnceExactly();
        }
        
        //public void DeleteUser(string id)
        //{
        //}

        //public void GetUserByEmail(string email)
        //{
        //}

        //public void UpdateUser([FromBody] UserViewModel userModel)
        //{
        //}

        //public void UpdateGender(string userId, Gender gender)
        //{
        //}

        //public void SearchUsers(string searchString)
        //{
        //}
    }
}