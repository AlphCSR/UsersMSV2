using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersMS.Core.Repositories;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Infrastructure.Repositories;
using UsersMS.Commons.Enums;
using Xunit;

namespace UsersMS.Test.UsersMS.Infrastructure.Test.Repositories
{
    public class UserRepositoryTest
    {
        private User GetUser()
        {
            return new User("a@b.com", "p", "d", "n", "l", "t", "dir", UserRole.Administrador, UserState.Active);
        }

        [Fact]
        public async Task AddAsync_Should_Add_User_Successfully()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UsersDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new UsersDbContext(options);
            var userRepository = new UserRepository(context);
            var user = GetUser();

            // Act
            await userRepository.AddAsync(user);

            // Assert
            Assert.Single(context.Users);
            Assert.Equal(user.Email, context.Users.First().Email);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_User_When_Found()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UsersDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new UsersDbContext(options);
            var userRepository = new UserRepository(context);
            var user = GetUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            await userRepository.DeleteAsync(user.Id);

            // Assert
            Assert.Empty(context.Users);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_User()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UsersDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new UsersDbContext(options);
            var userRepository = new UserRepository(context);
            var user = GetUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            user.Email = "updated@mail.com";
            await userRepository.UpdateAsync(user);

            // Assert
            Assert.Single(context.Users);
            Assert.Equal("updated@mail.com", context.Users.First().Email);
        }
    }
}
