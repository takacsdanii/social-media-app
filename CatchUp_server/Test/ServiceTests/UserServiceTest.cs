using CatchUp_server.Db;
using CatchUp_server.Interfaces.UserContentServices;
using CatchUp_server.Models.UserModels;
using CatchUp_server.Services.UserServices;
using CatchUp_server.ViewModels.UserViewModel;
using CatchUp_server.ViewModels.UserViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CatchUp_Test.ServiceTests
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly IMediaFoldersService _fakeMediaFoldersService;
        private IReadOnlyCollection<User> mockedUsers;

        public UserServiceTest()
        {
            _fakeMediaFoldersService = A.Fake<IMediaFoldersService>();
            var dbContext = GetDbContext();

            _userService = new UserService(dbContext, _fakeMediaFoldersService);
        }

        private ApiDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApiDbContext(options);
            context.Database.EnsureCreated();

            MockUsers();
            if(!context.Users.Any())
            {
                context.Users.AddRange(mockedUsers);
                context.SaveChanges();
            }
            return context;
        }

        [Fact]
        public void GetUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var id = "i2";
            var expectedUser = mockedUsers.First(u => u.Id == id);

            // Act
            var result = _userService.GetUser(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserViewModel>();
            result.Id.Should().Be(expectedUser.Id);
            result.UserName.Should().Be(expectedUser.UserName);
            result.FirstName.Should().Be(expectedUser.FirstName);
            result.LastName.Should().Be(expectedUser.LastName);
            result.ProfilePicUrl.Should().Be(expectedUser.ProfilePicUrl);
            result.CoverPicUrl.Should().Be(expectedUser.CoverPicUrl);
            result.Email.Should().Be(expectedUser.Email);
        }

        [Fact]
        public void GetUser_ShouldReturnNull_WhenUserDoesntExist()
        {
            // Arrange
            var nonExistingId = "nonexisting-id";
            var invalidId = "";

            // Act
            var resultForNonExisting = _userService.GetUser(nonExistingId);
            var resultForInvalidId = _userService.GetUser(invalidId);

            // Assert
            resultForNonExisting.Should().BeNull();
            resultForInvalidId.Should().BeNull();
        }

        [Fact]
        public void List_ShouldReturnAll()
        {
            // Arrange

            // Act
            var result = _userService.List();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(mockedUsers.Count);
            result.Should().BeEquivalentTo(mockedUsers, options => options
                .ComparingByMembers<UserViewModel>()
                .ExcludingMissingMembers()
                .WithStrictOrdering()
            );
        }

        [Fact]
        public void DeleteUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var id = "i2";
            var expectedUser = mockedUsers.First(u => u.Id == id);
            var initialUserCount = _userService.List().Count;

            // Act
            var result = _userService.DeleteUser(id);
            var remainingUsers = _userService.List();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
            result.Id.Should().Be(expectedUser.Id);
            result.UserName.Should().Be(expectedUser.UserName);
            result.FirstName.Should().Be(expectedUser.FirstName);
            result.LastName.Should().Be(expectedUser.LastName);
            result.ProfilePicUrl.Should().Be(expectedUser.ProfilePicUrl);
            result.CoverPicUrl.Should().Be(expectedUser.CoverPicUrl);
            result.Email.Should().Be(expectedUser.Email);

            remainingUsers.Should().HaveCount(initialUserCount - 1);
            remainingUsers.Any(u => u.Id == id).Should().BeFalse();

            A.CallTo(() => _fakeMediaFoldersService.DeleteUserMediaFolders(id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void DeleteUser_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistingId = "nonexistent-id";

            // Act
            var result = _userService.DeleteUser(nonExistingId);

            // Assert
            result.Should().BeNull();
            A.CallTo(() => _fakeMediaFoldersService.DeleteUserMediaFolders(nonExistingId)).MustNotHaveHappened();
        }


        [Fact]
        public void GetUserByEmail_ShouldReturnUserViewModel_WhenEmailExists()
        {
            // Arrange
            var email = "peter.parker@dailybugle.com";
            var expectedUser = mockedUsers.First(u => u.FirstName == "Peter" && u.LastName == "Parker");

            // Act
            var result = _userService.GetUserByEmail(email);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserViewModel>();
            result.Id.Should().Be(expectedUser.Id);
            result.FirstName.Should().Be(expectedUser.FirstName);
            result.LastName.Should().Be(expectedUser.LastName);
            result.UserName.Should().Be(expectedUser.UserName);
            result.ProfilePicUrl.Should().Be(expectedUser.ProfilePicUrl);
            result.CoverPicUrl.Should().Be(expectedUser.CoverPicUrl);
            result.Email.Should().Be(expectedUser.Email);
        }

        [Fact]
        public void GetUserByEmail_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Arrange
            var nonExistentEmail = "nonexistent.email@example.com";

            // Act
            var result = _userService.GetUserByEmail(nonExistentEmail);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetUserByEmail_ShouldReturnNull_WhenEmailIsNullOrEmpty()
        {
            // Arrange
            var emptyEmail = "";
            var nullEmail = (string)null;

            // Act
            var resultForEmptyEmail = _userService.GetUserByEmail(emptyEmail);
            var resultForNullEmail = _userService.GetUserByEmail(nullEmail);

            // Assert
            resultForEmptyEmail.Should().BeNull();
            resultForNullEmail.Should().BeNull();
        }

        [Fact]
        public void UpdateUser_ShouldReturnSuccess_WhenValidDataProvided()
        {
            // Arrange
            var userId = "i2";
            var updatedUserModel = new UserViewModel
            {
                Id = userId,
                FirstName = "Peter",
                LastName = "Parker",
                UserName = "SpiderManUpdated",
                Email = "spidermanupdated@marvel.com",
            };

            // Act
            var result = _userService.UpdateUser(updatedUserModel);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Should().Be(IdentityResult.Success);

            var updatedUser = _userService.GetUser(userId);
            updatedUser.Should().NotBeNull();
            updatedUser.FirstName.Should().Be(updatedUserModel.FirstName);
            updatedUser.LastName.Should().Be(updatedUserModel.LastName);
            updatedUser.UserName.Should().Be(updatedUserModel.UserName);
        }

        [Fact]
        public void UpdateUser_ShouldFail_WhenUserNameOrEmailIsAlreadyTaken()
        {
            // Arrange
            var userId = "i2";
            var existingUserName = "bbilbo";
            var userModel = new UserViewModel
            {
                Id = userId,
                UserName = existingUserName,
                Email = "newemail@example.com"
            };

            // Act
            var result = _userService.UpdateUser(userModel);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Description == "This username is already in use.");
        }

        [Fact]
        public void UpdateUser_ShouldFail_WhenEmailIsAlreadyTaken()
        {
            // Arrange
            var userId = "i2";
            var existingEmail = "bilbo.baggins@ring.com";
            var userModel = new UserViewModel
            {
                Id = userId,
                UserName = "SpiderManUpdated",
                Email = existingEmail
            };

            // Act
            var result = _userService.UpdateUser(userModel);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Description == "This email is already in use.");
        }

        [Fact]
        public void UpdateUser_ShouldFail_WhenUserNotFound()
        {
            // Arrange
            var nonExistingId = "nonexistent-id";
            var userModel = new UserViewModel
            {
                Id = nonExistingId,
                UserName = "NonExistentUser",
                Email = "nonexistent@example.com"
            };

            // Act
            var result = _userService.UpdateUser(userModel);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Description == "User not found.");
        }

        [Fact]
        public void UpdateGender_ShouldReturnUpdatedGender_WhenUserExists()
        {
            // Arrange
            var userId = "i2";
            var newGender = Gender.Other;

            // Act
            var result = _userService.UpdateGender(userId, newGender);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(newGender);

            var updatedUser = _userService.GetUser(userId);
            updatedUser.Gender.Should().Be(newGender);
        }

        [Fact]
        public void UpdateGender_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistingUserId = "nonexistent-user-id";
            var newGender = Gender.Male;

            // Act
            var result = _userService.UpdateGender(nonExistingUserId, newGender);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void SearchUsers_ShouldReturnMatchingUsers_WhenSearchStringMatchesUserDetails()
        {
            // Arrange
            var searchString = "bilb";
            var expectedUser = mockedUsers.First(u => u.UserName.Contains(searchString)
                                        || u.LastName.Contains(searchString)
                                        || u.FirstName.Contains(searchString)
                                );

            // Act
            var result = _userService.SearchUsers(searchString);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Should().BeOfType<SearchUserViewModel>();
            result.Should().BeOfType<List<SearchUserViewModel>>();
            result.First().Id.Should().Be(expectedUser.Id);
            result.First().UserName.Should().Be(expectedUser.UserName);
            result.First().FirstName.Should().Be(expectedUser.FirstName);
            result.First().LastName.Should().Be(expectedUser.LastName);
            result.First().ProfilePicUrl.Should().Be(expectedUser.ProfilePicUrl);
        }

        [Fact]
        public void SearchUsers_ShouldReturnEmptyList_WhenNoUserMatchesSearchString()
        {
            // Arrange
            var searchString = "NonExistentUser";

            // Act
            var result = _userService.SearchUsers(searchString);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void SearchUsers_ShouldReturnMultipleUsers_WhenSearchStringMatchesMultipleUsers()
        {
            // Arrange
            var searchString = "an";
            // Act
            var result = _userService.SearchUsers(searchString);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(1); 
            result.Any(u => u.Id == "i2").Should().BeTrue();
            result.Any(u => u.Id == "i3").Should().BeTrue();
        }

        private void MockUsers()
        {
            mockedUsers = new List<User>()
            {
                new User
                {
                    Id = "i1",
                    UserName = "bbilbo",
                    Email = "bilbo.baggins@ring.com",
                    Gender = Gender.Male,
                    FirstName = "Bilbo",
                    LastName = "Baggins",
                    ProfilePicUrl = "prof",
                    CoverPicUrl = "cov"
                },
                new User
                {
                    Id = "i2",
                    UserName = "SpiderMan",
                    Email = "peter.parker@dailybugle.com",
                    Gender = Gender.Male,
                    FirstName = "Peter",
                    LastName = "Parker",
                    ProfilePicUrl = "prof",
                    CoverPicUrl = "cov"
                },
                new User
                {
                    Id = "i3",
                    UserName = "Batman",
                    FirstName = "Bruce",
                    LastName = "Wayne",
                    ProfilePicUrl = "prof",
                    CoverPicUrl = "cov"
                },
                new User
                {
                    Id = "i4",
                    UserName = "eminem",
                    FirstName = "The Real",
                    LastName = "Slim Shady",
                    ProfilePicUrl = "prof",
                    CoverPicUrl = "cov"
                },
            };
        }
    }
}
