/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Infrastructure.Repositories;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.Exceptions;
using UsersMS.Commons.Enums;

[TestClass]
public class AdministradorRepositoryTests
{
    private UsersDbContext _dbContext;
    private AdministratorRepository _repositorio;

    public AdministradorRepositoryTests()
    { // Inicializar con un valor por defecto
      _dbContext = null!; 
      _repositorio = null!; 
    }

    [TestInitialize]
    public void Initialize()
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new UsersDbContext(options);
        _repositorio = new AdministratorRepository(_dbContext);
    }

    [TestMethod]
    public async Task AddAsync_DeberiaAgregarAdministrador()
    {
        // Arrange
        var administrador = new Administrator { 
            AdministratorId = Guid.NewGuid(), 
            Email = "leo@gmail.com", 
            LastName="Santos", 
            Id = "27904398",  
            Name ="Leoanrdo",
            Phone = "123456789",
            Address = "123 Main St",
            Password = "1234",
            Role = 0, 
            State = 0};

        // Act
        await _repositorio.AddAsync(administrador);

        // Assert
        var administradorAgregado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId);
        Assert.IsNotNull(administradorAgregado);
    }

    [TestMethod]
    [ExpectedException(typeof(AdministratorNotFoundException))]
    public async Task DeleteAsync_DeberiaLanzarExcepcionSiAdministradorNoEsEncontrado()
    { // Arrange
      var administradorId = Guid.NewGuid(); 
      // Act
      await _repositorio.DeleteAsync(administradorId); 
    }

    [TestMethod]
    public async Task DeleteAsync_DeberiaEliminarAdministrador()
    { // Arrange
        var administrador = new Administrator
        {
            AdministratorId = Guid.NewGuid(),
            Email = "leo@gmail.com",
            LastName = "Santos",
            Id = "27904398",
            Name = "Leoanrdo",
            Password = "1234",
            Phone = "123456789",
            Address = "123 Main St",
            Role = 0,
            State = 0
        };
        await _dbContext.Administrators.AddAsync(administrador); 
        await _dbContext.SaveChangesAsync(); 
        
     // Act
     await _repositorio.DeleteAsync(administrador.AdministratorId); 
        
     // Assert
     var administradorEliminado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId); 
     Assert.IsNull(administradorEliminado); 
    }


    [TestMethod]
    public async Task UpdateAsync_DeberiaActualizarNombreAdministrador()
    { // Arrange
        var administrador = new Administrator
        {
            AdministratorId = Guid.NewGuid(),
            Email = "leo@gmail.com",
            LastName = "Santos",
            Id = "27904398",
            Phone = "123456789",
            Address = "123 Main St",
            Name = "Leoanrdo",
            Password = "1234",
            Role = 0,
            State = 0
        };

        await _dbContext.Administrators.AddAsync(administrador); 
        await _dbContext.SaveChangesAsync(); 
        administrador.Name = "Nombre Actualizado"; 
     
     // Act
     await _repositorio.UpdateAsync(administrador); 
        
     // Assert
     
        var administradorActualizado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId); 
        Assert.AreEqual("Nombre Actualizado", administradorActualizado?.Name); 
    }

    [TestMethod]
    public async Task UpdateAsync_DeberiaActualizarLastNameAdministrador()
    { // Arrange
        var administrador = new Administrator
        {
            AdministratorId = Guid.NewGuid(),
            Email = "leo@gmail.com",
            LastName = "Santo",
            Id = "27904398",
            Phone = "123456789",
            Address = "123 Main St",
            Name = "Leoanrdo",
            Password = "1234",
            Role = 0,
            State = 0
        };

        await _dbContext.Administrators.AddAsync(administrador);
        await _dbContext.SaveChangesAsync();
        administrador.LastName = "LastName Actualizado";

        // Act
        await _repositorio.UpdateAsync(administrador);

        // Assert

        var administradorActualizado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId);
        Assert.AreEqual("LastName Actualizado", administradorActualizado?.LastName);
    }

    [TestMethod]
    public async Task UpdateAsync_DeberiaActualizarEmailAdministrador()
    { // Arrange
        var administrador = new Administrator
        {
            AdministratorId = Guid.NewGuid(),
            Email = "leo@gmail.com",
            LastName = "Santos",
            Id = "27904398",
            Phone = "123456789",
            Address = "123 Main St",
            Name = "Leoanrdo",
            Password = "1234",
            Role = 0,
            State = 0
        };

        await _dbContext.Administrators.AddAsync(administrador);
        await _dbContext.SaveChangesAsync();
        administrador.Email = "Email Actualizado";

        // Act
        await _repositorio.UpdateAsync(administrador);

        // Assert

        var administradorActualizado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId);
        Assert.AreEqual("Email Actualizado", administradorActualizado?.Email);
    }

    [TestMethod]
    public async Task UpdateAsync_DeberiaActualizarIdAdministrador()
    { // Arrange
        var administrador = new Administrator
        {
            AdministratorId = Guid.NewGuid(),
            Email = "leo@gmail.com",
            LastName = "Santos",
            Id = "27904398",
            Phone = "123456789",
            Address = "123 Main St",
            Name = "Leoanrdo",
            Password = "1234",
            Role = 0,
            State = 0
        };

        await _dbContext.Administrators.AddAsync(administrador);
        await _dbContext.SaveChangesAsync();
        administrador.Id = "Id Actualizada";

        // Act
        await _repositorio.UpdateAsync(administrador);

        // Assert

        var administradorActualizado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId);
        Assert.AreEqual("Id Actualizada", administradorActualizado?.Id);
    }

    [TestMethod]
    public async Task UpdateAsync_DeberiaActualizarDepartamentoIdAdministrador()
    { // Arrange
        var administrador = new Administrator
        {
            AdministratorId = Guid.NewGuid(),
            Email = "leo@gmail.com",
            LastName = "Santos",
            Id = "27904398",
            Phone = "123456789",
            Address = "123 Main St",
            Name = "Leoanrdo",
            Password = "1234",
            Role = 0,
            State = 0
        };

        await _dbContext.Administrators.AddAsync(administrador);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repositorio.UpdateAsync(administrador);

        // Assert

        var administradorActualizado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId);
    }

    [TestMethod]
    public async Task UpdateAsync_DeberiaActualizarEmpresaIdAdministrador()
    { // Arrange
        var administrador = new Administrator
        {
            AdministratorId = Guid.NewGuid(),
            Email = "leo@gmail.com",
            LastName = "Santos",
            Id = "27904398",
            Phone = "123456789",
            Address = "123 Main St",
            Name = "Leoanrdo",
            Password = "1234",
            Role = 0,
            State = 0
        };

        await _dbContext.Administrators.AddAsync(administrador);
        await _dbContext.SaveChangesAsync();

        // Act

        // Assert

        var administradorActualizado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId);
    }

    [TestMethod]
    public async Task UpdateAsync_DeberiaActualizarPasswordAdministrador()
    { // Arrange
        var administrador = new Administrator
        {
            AdministratorId = Guid.NewGuid(),
            Email = "leo@gmail.com",
            LastName = "Santos",
            Id = "27904398",
            Phone = "123456789",
            Address = "123 Main St",
            Name = "Leoanrdo",
            Password = "1234",
            Role = 0,
            State = 0
        };

        await _dbContext.Administrators.AddAsync(administrador);
        await _dbContext.SaveChangesAsync();
        administrador.Password = "Password Actualizada";

        // Act
        await _repositorio.UpdateAsync(administrador);

        // Assert

        var administradorActualizado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId);
        Assert.AreEqual("Password Actualizada", administradorActualizado?.Password);
    }

    [TestMethod]
    public async Task UpdateAsync_DeberiaActualizarStateAdministrador()
    { // Arrange
        var administrador = new Administrator
        {
            AdministratorId = Guid.NewGuid(),
            Email = "leo@gmail.com",
            LastName = "Santos",
            Id = "27904398",
            Phone = "123456789",
            Address = "123 Main St",
            Name = "Leoanrdo",
            Password = "1234",
            Role = 0,
            State = 0
        };

        await _dbContext.Administrators.AddAsync(administrador);
        await _dbContext.SaveChangesAsync();
        administrador.State = UserState.Inactive;

        // Act
        await _repositorio.UpdateAsync(administrador);

        // Assert

        var administradorActualizado = await _dbContext.Administrators.FindAsync(administrador.AdministratorId);
        Assert.AreEqual(UserState.Inactive, administradorActualizado?.State);
    }

    [TestMethod]
    public async Task GetByIdAsync_DeberiaRetornarAdministradorPorId()
    { // Arrange
      var Id = Guid.NewGuid();
        var administrador = new Administrator
        {
            AdministratorId = Id,
            Email = "leo@gmail.com",
            LastName = "Santos",
            Id = "27904398",
            Phone = "123456789",
            Address = "123 Main St",
            Name = "Leoanrdo",
            Password = "1234",
            Role = 0,
            State = 0
        };
        await _dbContext.Administrators.AddAsync(administrador); await _dbContext.SaveChangesAsync(); 
        // Act
        var resultado = await _repositorio.GetByIdAsync(Id); 
        // Assert
        Assert.AreEqual(administrador, resultado); 
    }

    [TestMethod]
    public async Task GetAllAsync_DeberiaRetornarTodosLosAdministrators()
    { // Arrange
      var administradores = new List<Administrator> { new Administrator
            {
                AdministratorId = Guid.NewGuid(),
                Email = "leo@gmail.com",
                LastName = "Santos",
                Id = "27904398",
                Phone = "123456789",
                Address = "123 Main St",
                Name = "Leoanrdo",
                Password = "1234",
                Role = 0,
                State = 0
            }, 
            new Administrator 
            {
                AdministratorId = Guid.NewGuid(),
                Email = "richard@gmail.com",
                LastName = "morales",
                Id = "2554489",
                Phone = "123456789",
                Address = "123 Main St",
                Name = "Richardison",
                Password = "7841a",
                Role = 0,
                State = UserState.Inactive
            } 
      };

        var todosLosAdministrators = await _dbContext.Administrators.ToListAsync(); 
        _dbContext.Administrators.RemoveRange(todosLosAdministrators);

        await _dbContext.Administrators.AddRangeAsync(administradores);
        await _dbContext.SaveChangesAsync(); 
        
     //Act
     var resultado = await _repositorio.GetAllAsync(); 
        
     // Assert
     Assert.AreEqual(administradores.Count, resultado?.Count); 
    }
}
*/


