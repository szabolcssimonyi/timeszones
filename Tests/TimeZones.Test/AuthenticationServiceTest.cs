using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TimeZones.Extensibility;
using TimeZones.Extensibility.Dto;
using TimeZones.Service;

namespace TimeZones.Test
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        public Mock<IOptions<Configuration>> mockOption;
        public Mock<ILogger<AuthenticationService>> mockLogger;
        public Mock<UserManager<IdentityUser>> mockUserManager;
        private IdentityUser testUser = new IdentityUser
        {
            UserName = "Test Username",
            Email = "test@useremail.com",
            Id = "test id",
            PhoneNumber = "111111",
        };
        private LoginDto loginDto = new LoginDto
        {
            UserName = "Test Username",
            Password = "TestPassword"
        };

        [SetUp]
        public void Setup()
        {
            mockOption = new Mock<IOptions<Configuration>>();
            mockLogger = new Mock<ILogger<AuthenticationService>>();
            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            mockUserManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(mock => mock.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(testUser));
        }

        [TestCase(TestName = "AuthenticationService - Login: request input is null, should return null")]
        public async Task Login_RequestInputIsNull_ShouldCallLogAndReturnNull()
        {
            var service = new AuthenticationService(mockOption.Object, mockLogger.Object, mockUserManager.Object);
            var result = await service.Login(null);
            mockUserManager.Verify(mock => mock.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
            Assert.IsNull(result);
        }

        [TestCase(TestName = "AuthenticationService - Login: request input is valid, should return user")]
        public async Task Login_RequestInputIsValid_ShouldReturnUser()
        {
            mockUserManager.Setup(mock => mock.FindByNameAsync(loginDto.UserName)).Returns(Task.FromResult(testUser));
            mockUserManager.Setup(mock => mock.CheckPasswordAsync(testUser, loginDto.Password))
                .Returns(Task.FromResult(true));

            var service = new AuthenticationService(mockOption.Object, mockLogger.Object, mockUserManager.Object);
            var result = await service.Login(loginDto);

            mockUserManager.Verify(mock => mock.CheckPasswordAsync(testUser, loginDto.Password), Times.Once);
            Assert.AreEqual(testUser, result);
        }
    }
}
