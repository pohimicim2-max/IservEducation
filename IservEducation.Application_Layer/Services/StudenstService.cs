using CSharpFunctionalExtensions;
using IservEducation.Application_Layer.Interfaces;
using IservEducation.Domain_Layer.Models;

namespace IservEducation.Application_Layer.Services;

public class StudenstService : IStudentsService
{
	private readonly IStudentsRepository _studentRepository;

	public StudenstService(IStudentsRepository studentsRepository)
	{
		_studentRepository = studentsRepository;
	}

	public async Task<Result<Guid>> CreateAsync(string firstName, string lastName, string? middleName = null, Guid? groupId = null, int countCodeCoin = 0)
	{
		if (string.IsNullOrWhiteSpace(firstName))
			return Result.Failure<Guid>("FirstName are required");

		if (string.IsNullOrWhiteSpace(lastName))
			return Result.Failure<Guid>("LastName are required");


		var normalizedFirstName = firstName?.Trim() ?? string.Empty;
		var normalizedLastName = lastName?.Trim() ?? string.Empty;
		var normalizedMiddleName = middleName?.Trim();

		// 1) Создаём доменную модель через фабрику
		var result = Student.Create(Guid.NewGuid(), normalizedFirstName, normalizedLastName, normalizedMiddleName, groupId, countCodeCoin);

		if (!result.IsSuccess)
			return Result.Failure<Guid>(result.Error);

		var student = result.Value;

		// 2) Сохраняем
		await _studentRepository.AddAsync(student);

		return Result.Success(student.Id);
	}

	public async Task<Result<Student>> GetByIdAsync(Guid id)
	{
		if (id == Guid.Empty)
			return Result.Failure<Student>("id are required");

		var student = await _studentRepository.GetByIdAsync(id);

		return Result.Success<Student>(student);
	}

	public async Task<Result<Guid>> DeleteAsync(Guid id)
	{
		if (id == Guid.Empty)
			return Result.Failure<Guid>("id are required");

		var deletedId = await _studentRepository.DeleteAsync(id);

		return Result.Success<Guid>(deletedId);
	}
}
