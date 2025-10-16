using CSharpFunctionalExtensions;
using IservEducation.Application_Layer.Interfaces;

namespace IservEducation.Application_Layer.Services;

public class TeachersService : ITeachersService
{
	private readonly ITeachersRepository _teacherRepository;
	private readonly IPasswordHasher _passwordHasher;

	public TeachersService(ITeachersRepository teacherRepository, IPasswordHasher passwordHasher)
	{
		_teacherRepository = teacherRepository
			?? throw new ArgumentNullException(nameof(teacherRepository));

		_passwordHasher = passwordHasher
			?? throw new ArgumentNullException(nameof(passwordHasher));
	}

	public async Task<Result<Guid>> CreateAsync(string login, string password, string firstName, string lastName, string? middleName = null)
	{
		if (string.IsNullOrWhiteSpace(login))
			return Result.Failure<Guid>("Login are required");

		if (string.IsNullOrWhiteSpace(password))
			return Result.Failure<Guid>("Password are required");

		if (string.IsNullOrWhiteSpace(firstName))
			return Result.Failure<Guid>("FirstName are required");

		if (string.IsNullOrWhiteSpace(lastName))
			return Result.Failure<Guid>("LastName are required");


		var normalizedLogin = login.Trim().ToLowerInvariant();
		var normalizedFirstName = firstName?.Trim() ?? string.Empty;
		var normalizedLastName = lastName?.Trim() ?? string.Empty;
		var normalizedMiddleName = middleName?.Trim();

		// 1) Уникальность логина
		var existingLogin = await _teacherRepository.GetByLoginAsync(normalizedLogin);

		if (existingLogin != null)
			return Result.Failure<Guid>("Login already in use");

		// 2) Хешируем пароль
		var passwordHash = _passwordHasher.Hash(password);

		// 3) Создаём доменную модель через фабрику
		var result = Teacher.Create(Guid.NewGuid(), normalizedLogin, passwordHash, normalizedFirstName, normalizedLastName, normalizedMiddleName);

		if (!result.IsSuccess)
			return Result.Failure<Guid>(result.Error);

		var teacher = result.Value;

		// 4) Сохраняем
		await _teacherRepository.AddAsync(teacher);

		return Result.Success(teacher.Id);
	}

	public async Task<Result<Teacher>> LoginAsync(string login, string password)
	{
		if (string.IsNullOrWhiteSpace(login))
			return Result.Failure<Teacher>("Login are required");

		if (string.IsNullOrWhiteSpace(password))
			return Result.Failure<Teacher>("Password are required");

		var normalizedLogin = login.Trim().ToLowerInvariant();

		// Получаем преподавателя по логину
		var teacher = await _teacherRepository.GetByLoginAsync(login);

		if (teacher == null)
			return Result.Failure<Teacher>("User with this login is not found");

		// Проверяем пароль

		var equalPassword = _passwordHasher.Verify(teacher.PasswordHash, password);

		if (!equalPassword)
			return Result.Failure<Teacher>("Invalid password");


		return Result.Success<Teacher>(teacher);
	}

	public async Task<Result<Teacher>> GetByIdAsync(Guid id)
	{
		if (id == Guid.Empty)
			return Result.Failure<Teacher>("id are required");

		var teacher = await _teacherRepository.GetByIdAsync(id);

		return Result.Success<Teacher>(teacher);
	}
}


