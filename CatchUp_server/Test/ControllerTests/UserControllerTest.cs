using CatchUp_server.Controllers;
using CatchUp_server.Services.UserServices;
using FluentAssertions;
using FakeItEasy;

namespace CatchUp_Test.ControllerTests
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly UserService _fakeService;

        public UserControllerTest()
        {
            _fakeService = A.Fake<UserService>();
            _controller = new UserController(_fakeService);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}