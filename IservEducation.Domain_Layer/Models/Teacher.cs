using CSharpFunctionalExtensions;

public sealed class Teacher
{
	public Guid Id { get; private set; }
	public string Login { get; private set; } = string.Empty;
	public string PasswordHash { get; private set; } = string.Empty;
	public string FirstName { get; private set; } = string.Empty;
	public string LastName { get; private set; } = string.Empty;
	public string? MiddleName { get; private set; } = string.Empty;

	private readonly List<Guid> _lessonIds = new(); // приватный список проставленных уроков
	public IReadOnlyCollection<Guid> LessonIds => _lessonIds.AsReadOnly(); // публичный список для чтения уроков

	private Teacher() { }

	public static Result<Teacher> Create(Guid id, string login, string passwordHash, string firstName, string lastName, string? middleName = null)
	{
		// 
		// Валидация данных (в будущем реализовать класс UserValidator)
		//
		if (id == Guid.Empty)
			return Result.Failure<Teacher>("Id must be a non-empty Guid");

		if (string.IsNullOrWhiteSpace(login))
			return Result.Failure<Teacher>("Login is required");

		if (login.Length < 3 || login.Length > 100)
			return Result.Failure<Teacher>("Login length must be between 3 and 100 characters");

		if (string.IsNullOrWhiteSpace(passwordHash))
			return Result.Failure<Teacher>("PasswordHash is required");

		if (string.IsNullOrWhiteSpace(firstName))
			return Result.Failure<Teacher>("FirstName is required");

		if (firstName.Length > 150)
			return Result.Failure<Teacher>("FirstName length must be <= 150 characters");

		if (string.IsNullOrWhiteSpace(lastName))
			return Result.Failure<Teacher>("LastName is required");

		if (lastName.Length > 150)
			return Result.Failure<Teacher>("LastName length must be <= 150 characters");

		if (middleName != null && middleName.Length > 150)
			return Result.Failure<Teacher>("MiddleName length must be <= 150 characters");

		var teacher = new Teacher
		{
			Id = id,
			Login = login,
			PasswordHash = passwordHash,
			FirstName = firstName,
			LastName = lastName,
			MiddleName = middleName ?? string.Empty
		};

		return Result.Success(teacher);
	}
	// 
	// Обновление профиля
	// 
	public void UpdatePasswordHash(string newPasswordHash)
	{
		if (string.IsNullOrWhiteSpace(newPasswordHash))
			throw new ArgumentException("newHash is required", nameof(newPasswordHash));

		if (newPasswordHash == PasswordHash)
			return;

		PasswordHash = newPasswordHash;
	}
	public void UpdateFullName(string firstName, string lastName, string? middleName = null)
	{
		if (string.IsNullOrWhiteSpace(firstName))
			throw new ArgumentException("FirstName is required", nameof(firstName));

		if (string.IsNullOrWhiteSpace(lastName))
			throw new ArgumentException("LastName is required", nameof(lastName));

		FirstName = firstName;
		LastName = lastName;
		MiddleName = middleName ?? string.Empty;
	}
	//
	// Работа с уроками
	//
	public void AddLesson(Guid lessonId)
	{
		if (lessonId == Guid.Empty)
			throw new ArgumentException("lessonId must be a non-empty Guid", nameof(lessonId));

		if (_lessonIds.Contains(lessonId))
			return;

		_lessonIds.Add(lessonId);
	}
	public bool RemoveLesson(Guid lessonId)
	{
		if (lessonId == Guid.Empty)
			return false;

		return _lessonIds.Remove(lessonId);
	}
	public void ReplaceLessons(IEnumerable<Guid> lessonIds)
	{
		if (lessonIds == null)
			throw new ArgumentNullException(nameof(lessonIds));

		var normalized = lessonIds
			.Where(g => g != Guid.Empty)
			.Distinct()
			.ToList();

		if (_lessonIds.Count == normalized.Count && !_lessonIds.Except(normalized).Any())
			return;

		_lessonIds.Clear();
		_lessonIds.AddRange(normalized);
	}
	public bool HaveLesson(Guid lessonId)
	{
		return _lessonIds.Contains(lessonId);
	}
}