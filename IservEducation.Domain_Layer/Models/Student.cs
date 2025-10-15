using CSharpFunctionalExtensions;

namespace IservEducation.Domain_Layer.Models;

public sealed class Student
{
	public Guid Id { get; private set; }
	public string FirstName { get; private set; } = string.Empty;
	public string LastName { get; private set; } = string.Empty;
	public string MiddleName { get; private set; } = string.Empty;

	public Guid GroupId { get; private set; }

	private readonly List<Guid> _lessonIds = new();
	public IReadOnlyCollection<Guid> LessonIds => _lessonIds.AsReadOnly();

	private Student() { }

	// ---------------------------
	// Factory Method
	// ---------------------------
	public static Result<Student> Create(
		Guid id,
		string firstName,
		string lastName,
		Guid groupId,
		string? middleName = null)
	{
		if (id == Guid.Empty)
			return Result.Failure<Student>("Id must be a non-empty Guid");

		if (string.IsNullOrWhiteSpace(firstName))
			return Result.Failure<Student>("FirstName is required");

		if (firstName.Length > 150)
			return Result.Failure<Student>("FirstName length must be <= 150 characters");

		if (string.IsNullOrWhiteSpace(lastName))
			return Result.Failure<Student>("LastName is required");

		if (lastName.Length > 150)
			return Result.Failure<Student>("LastName length must be <= 150 characters");

		if (groupId == Guid.Empty)
			return Result.Failure<Student>("GroupId must be a non-empty Guid");

		var student = new Student
		{
			Id = id,
			FirstName = firstName,
			LastName = lastName,
			MiddleName = middleName ?? string.Empty,
			GroupId = groupId
		};

		return Result.Success(student);
	}

	// ---------------------------
	// Methods for lessons
	// ---------------------------

	public Result AddLesson(Guid lessonId)
	{
		if (lessonId == Guid.Empty)
			return Result.Failure("LessonId must be non-empty");

		if (_lessonIds.Contains(lessonId))
			return Result.Failure("Student already assigned to this lesson");

		_lessonIds.Add(lessonId);
		return Result.Success();
	}

	public Result RemoveLesson(Guid lessonId)
	{
		if (!_lessonIds.Remove(lessonId))
			return Result.Failure("Student was not assigned to this lesson");

		return Result.Success();
	}

	public bool AttendsLesson(Guid lessonId)
		=> _lessonIds.Contains(lessonId);
}