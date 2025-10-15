using Xunit;
using System;
namespace IservEducation.Tests.Domain_Layer.Models;

public class TeacherTests
{
	[Fact] // Атрибут, говорящий xUnit, что это тест
	public void Create_WithValidData_ShouldReturnSuccess()
	{
		// Arrange (подготовка данных)
		var id = Guid.NewGuid();
		var login = "teacher123";
		var passwordHash = "hashed_pw";
		var firstName = "Иван";
		var lastName = "Иванов";

		// Act (выполняем действие)
		var result = Teacher.Create(id, login, passwordHash, firstName, lastName);

		// Assert (проверяем результат)
		Assert.True(result.IsSuccess); // Проверяем, что успех
		Assert.NotNull(result.Value);
		Assert.Equal(login, result.Value.Login);
		Assert.Equal(firstName, result.Value.FirstName);
		Assert.Equal(lastName, result.Value.LastName);
	}

	[Fact]
	public void Create_WithEmptyLogin_ShouldReturnFailure()
	{
		// Arrange
		var id = Guid.NewGuid();

		// Act
		var result = Teacher.Create(id, "", "hash", "Иван", "Иванов");

		// Assert
		Assert.False(result.IsSuccess);
		Assert.Contains("Login is required", result.Error);
	}
}
