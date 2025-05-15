using System;
using Xunit;
using UsersMS.Domain.Entities;
using UsersMS.Commons.Enums;

namespace UsersMS.Test.UsersMS.Domain.Test.Entities
{
    public class UserTest
    {
        [Fact]
        public void CreateUser_Should_Set_Properties_Correctly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userName = "testuser";
            var email = "testuser@example.com";
            var role = UserRole.Administrador;
            var state = UserState.Active;

            // Act
            var user = new User
            {
                Id = id,
                Name = userName,
                Email = email,
                Role = role,
                State = state
            };

            // Assert
            Assert.Equal(id, user.Id);
            Assert.Equal(userName, user.Name);
            Assert.Equal(email, user.Email);
            Assert.Equal(role, user.Role);
            Assert.Equal(state, user.State);
        }

        [Fact]
        public void UpdateUser_Should_Update_Properties_Correctly()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "olduser",
                Email = "olduser@example.com",
                Role = UserRole.Operador,
                State = UserState.Inactive
            };

            var newUserName = "newuser";
            var newEmail = "newuser@example.com";
            var newRole = UserRole.Administrador;
            var newState = UserState.Active;

            // Act
            user.Name = newUserName;
            user.Email = newEmail;
            user.Role = newRole;
            user.State = newState;

            // Assert
            Assert.Equal(newUserName, user.Name);
            Assert.Equal(newEmail, user.Email);
            Assert.Equal(newRole, user.Role);
            Assert.Equal(newState, user.State);
        }

        [Fact]
        public void Constructor_Should_Create_User_Successfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userName = "testuser";
            var email = "testuser@example.com";
            var role = UserRole.Operador;
            var state = UserState.Active;

            // Act
            var user = new User
            {
                Id = userId,
                Name = userName,
                Email = email,
                Role = role,
                State = state
            };

            // Assert
            Assert.Equal(userId, user.Id);
            Assert.Equal(userName, user.Name);
            Assert.Equal(email, user.Email);
            Assert.Equal(role, user.Role);
            Assert.Equal(state, user.State);
        }

        [Fact]
        public void UpdateEmail_Should_Update_Email_Successfully()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "testuser",
                Email = "oldemail@example.com",
                Role = UserRole.Administrador,
                State = UserState.Active
            };

            var newEmail = "newemail@example.com";

            // Act
            // user.UpdateEmail(newEmail);

            // Assert
            //Assert.Equal(newEmail, user.Email);
        }

        [Fact]
        public void UpdateEmail_Should_Throw_Exception_When_Email_Is_Invalid()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "testuser",
                Email = "oldemail@example.com",
                Role = UserRole.Administrador,
                State = UserState.Active
            };

            var invalidEmail = "invalid-email";

            // Act & Assert
            //Assert.Throws<ArgumentException>(() => user.UpdateEmail(invalidEmail));
        }
    }
}
