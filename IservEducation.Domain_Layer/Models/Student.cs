using CSharpFunctionalExtensions;

namespace IservEducation.Domain_Layer.Models;

public sealed class Student
{
	public Guid Id { get; private set; }

	public string FirstName { get; private set; } = string.Empty;
	public string LastName { get; private set; } = string.Empty;
	public string MiddleName { get; private set; } = string.Empty;

	private readonly List<Guid> _lessonsStatisticId = new();
	public IReadOnlyCollection<Guid>? LessonStatisticIds => _lessonsStatisticId.AsReadOnly();

	public Guid? GroupId { get; private set; }

	public int CountCodeCoin { get; private set; } = 0;
	

	private Student() { }

	public static Result<Student> Create(Guid id, string firstName, string lastName, string? middleName = null, Guid? groupId = null, int countCodeCoin = 0)
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

		if (countCodeCoin < 0 || countCodeCoin > 999)
			return Result.Failure<Student>("Count CodeCoin must be >= 0 and <= 999");

		var student = new Student
		{
			Id = id,
			FirstName = firstName,
			LastName = lastName,
			MiddleName = middleName ?? string.Empty,
			GroupId = groupId,
			CountCodeCoin = countCodeCoin
		};

		return Result.Success(student);
	}
	public void AddCodeCoin(int codeCoin)
	{
		if(codeCoin < 0 || codeCoin > 999)
			throw new ArgumentException("Ivalid value for add", nameof(codeCoin));

		CountCodeCoin += codeCoin;
	}
	public void SubtractCodeCoin(int codeCoin)
	{
		if(codeCoin < 0 || codeCoin > 999)
			throw new ArgumentException("Ivalid value for add", nameof(codeCoin));

		CountCodeCoin -= codeCoin;

		if (CountCodeCoin < 0)
			CountCodeCoin = 0;
	}
	public Result EnrollGroup(Guid groupId)
	{
		if (groupId == Guid.Empty)
			return Result.Failure("GroupId must be non-empty");

		GroupId = groupId;
		return Result.Success();
	}
	public void ReplaceLessonsStatistic(IEnumerable<Guid> lessonsStatisticId)
	{
		if (lessonsStatisticId == null)
			throw new ArgumentException("lessonsStatisticId must be a non-empty Guid", nameof(lessonsStatisticId));

		var normalized = lessonsStatisticId
			.Where(g => g != Guid.Empty)
			.Distinct()
			.ToList();

		if(_lessonsStatisticId.Count == normalized.Count && !_lessonsStatisticId.Except(normalized).Any())
			return;

		_lessonsStatisticId.Clear();
		_lessonsStatisticId.AddRange(normalized);
	}
	public void AddLessonStatistck(Guid lessonStatisticId)
	{
		if (lessonStatisticId == Guid.Empty)
			throw new ArgumentException("lessonStatisticId must be a non-empty Guid", nameof(lessonStatisticId));

		if (_lessonsStatisticId.Contains(lessonStatisticId))
			return;

		_lessonsStatisticId.Add(lessonStatisticId);
	}
	public bool RemoveLesson(Guid lessonId)
	{
		if (lessonId == Guid.Empty)
			return false;

		return _lessonsStatisticId.Remove(lessonId);
	}
	public bool AttendsLesson(Guid lessonId)
	{
		return _lessonsStatisticId.Contains(lessonId);
	} 
}