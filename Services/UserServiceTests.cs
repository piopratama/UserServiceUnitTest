using System.Collections.Generic;
using Moq;
using UserService.Models;
using UserService.Repositories;
using UserService.Services;
using Xunit;

namespace UserService.Tests.Services
{
	public class UserServiceTests
	{
		private readonly UserService.Services.UserService _userService;
		private readonly Mock<IUserRepository> _mockUserRepository;

		public UserServiceTests()
		{
			// Setup mock for IUserRepository
			_mockUserRepository = new Mock<IUserRepository>();

			// Initialize UserService with mock repository
			_userService = new UserService.Services.UserService(_mockUserRepository.Object);
		}

		[Fact]
		public void Authenticate_WithValidCredentials_ReturnsUser()
		{
			// Arrange
			var username = "testuser";
			var password = "password123";
			var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

			var user = new User { Username = username, PasswordHash = hashedPassword };

			// Setup mock repository to return the user
			_mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

			// Act
			var result = _userService.Authenticate(username, password);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(username, result.Username);
		}

		[Fact]
		public void Authenticate_WithInvalidCredentials_ReturnsNull()
		{
			// Arrange
			var username = "testuser";
			var wrongPassword = "wrongpassword";

			var user = new User { Username = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123") };

			// Setup mock repository to return the user
			_mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

			// Act
			var result = _userService.Authenticate(username, wrongPassword);

			// Assert
			Assert.Null(result);
		}
	}
}
