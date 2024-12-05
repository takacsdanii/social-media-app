using CatchUp_server.Controllers;
using FakeItEasy;
using CatchUp_server.Interfaces;
using CatchUp_server.ViewModels.UserViewModel;
using FluentAssertions;
using CatchUp_server.Models.UserModels;
using CatchUp_server.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

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
            fakeUser = new UserViewModel 
            { 
                Id = "user1", 
                UserName = "BilboBaggins", 
                Email = "bilbobaggins@gmai.com" ,
                Gender = Gender.Male,
            };
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
            result.Should().HaveCount(fakeUsers.Count);
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

            // Assert
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

            // Assert
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

            // Assert
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

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();

            A.CallTo(() => _fakeService.DeleteUser(userId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetUserByEmail_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            A.CallTo(() => _fakeService.GetUserByEmail(fakeUser.Email))
                .Returns(fakeUser);

            // Act
            var result = _controller.GetUserByEmail(fakeUser.Email);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeEquivalentTo(fakeUser);

            A.CallTo(() => _fakeService.GetUserByEmail(fakeUser.Email)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void GetUserByEmail_ShouldReturnNotFound_WhenUserDoesntExist()
        {
            // Arrange
            var email = "a@email.com";
            A.CallTo(() => _fakeService.GetUserByEmail(email))
                .Returns(null);

            // Act
            var result = _controller.GetUserByEmail(email);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();

            A.CallTo(() => _fakeService.GetUserByEmail(email)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void UpdateUser_ShouldSucceed()
        {
            // Arrange
            A.CallTo(() => _fakeService.UpdateUser(fakeUser))
                .Returns(IdentityResult.Success);

            // Act
            var result = _controller.UpdateUser(fakeUser);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeEquivalentTo(fakeUser);

            A.CallTo(() => _fakeService.UpdateUser(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void UpdateUser_ShouldNotSucceed()
        {
            // Arrange
            var fakeErrors = new[] { new IdentityError { Description = "ERROR" } };
            A.CallTo(() => _fakeService.UpdateUser(fakeUser))
                .Returns(IdentityResult.Failed(fakeErrors));

            // Act
            var result = _controller.UpdateUser(fakeUser);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
            var badrequest = result as BadRequestObjectResult;
            badrequest?.Value.Should().BeEquivalentTo(new { errors = fakeErrors.Select(e => e.Description).ToList() });

            A.CallTo(() => _fakeService.UpdateUser(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void UpdateGender_ShouldReturnOk_WhenGenderIsUpdated()
        {
            // Arrange
            A.CallTo(() => _fakeService.UpdateGender(fakeUser.Id, fakeUser.Gender))
                .Returns(fakeUser.Gender);

            // Act
            var result = _controller.UpdateGender(fakeUser.Id, fakeUser.Gender);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().Be(fakeUser.Gender);

            A.CallTo(() => _fakeService.UpdateGender(fakeUser.Id, fakeUser.Gender)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void UpdateGender_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            A.CallTo(() => _fakeService.UpdateGender(fakeUser.Id, fakeUser.Gender))
                .Returns(null);

            // Act
            var result = _controller.UpdateGender(fakeUser.Id, fakeUser.Gender);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();

            A.CallTo(() => _fakeService.UpdateGender(fakeUser.Id, fakeUser.Gender)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void SearchUsers_ShouldReturnAll_WhenListIsNotEmpty()
        {
            // Arrange
            var searchString = "ame";
            var users = new List<SearchUserViewModel>()
            {
                new SearchUserViewModel { Id = "id1", UserName = "name1" },
                new SearchUserViewModel { Id = "id4", UserName = "game2" }
            };
            A.CallTo(() => _fakeService.SearchUsers(searchString))
                .Returns(users);

            // Act
            var result = _controller.SearchUsers(searchString);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(users);
            A.CallTo(() => _fakeService.SearchUsers(searchString)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void SearchUsers_ShouldReturnEmptyList_WhenListIsEmpty()
        {
            // Arrange
            var searchString = "man";
            var users = new List<SearchUserViewModel>()
            {
                new SearchUserViewModel { Id = "id1", UserName = "name1" },
                new SearchUserViewModel { Id = "id4", UserName = "game2" }
            };
            A.CallTo(() => _fakeService.SearchUsers(searchString))
                .Returns(new List<SearchUserViewModel>());

            // Act
            var result = _controller.SearchUsers(searchString);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            A.CallTo(() => _fakeService.SearchUsers(searchString)).MustHaveHappenedOnceExactly();
        }
    }
}