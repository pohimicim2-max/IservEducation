using CSharpFunctionalExtensions;

namespace IservEducation.Domain_Layer.Models;

public sealed class Group
{
	public Guid Id { get; private set; }

	public string Name { get; private set; } = string.Empty;


	private readonly List<Guid> _studentId = new();
	public IReadOnlyCollection<Guid> StudentIds => _studentId.AsReadOnly();

	private Group() { }
	public static Result<Group> Create(Guid id, string name, IEnumerable<Guid>? studentsId = null)
	{
		if (id == Guid.Empty)
			return Result.Failure<Group>("Id must be a non-empty Guid");

		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<Group>("Name is required");

		if (name.Length > 300)
			return Result.Failure<Group>("Name length must be <= 300 characters");

		var group = new Group
		{
			Id = id,
			Name = name.Trim()
		};

		if (studentsId != null)
		{
			var normalized = studentsId
				.Where(g => g != Guid.Empty)
				.Distinct()
				.ToList();

			group._studentId.AddRange(normalized);
		}

		return Result.Success(group);
	}

	public Result AddStudent(Guid studentId)
	{
		if (studentId == Guid.Empty)
			return Result.Failure("StudentId must be a non-empty Guid");

		if (_studentId.Contains(studentId))
			return Result.Failure("Student already in group");

		_studentId.Add(studentId);
		return Result.Success();
	}

	public Result RemoveStudent(Guid lessonId)
	{
		if (lessonId == Guid.Empty)
			return Result.Failure("StudentId must be a non-empty Guid");

		var removed = _studentId.Remove(lessonId);

		if (!removed) 
			return Result.Failure("Student not found in group");

		return Result.Success();
	}
	public Result ReplaceStudents(IEnumerable<Guid> studentIds)
	{
		if (studentIds == null) 
			return Result.Failure("studentIds is required");

		var normalized = studentIds
			.Where(g => g != Guid.Empty)
			.Distinct()
			.ToList();

		bool isEqual = _studentId.Count == normalized.Count && !_studentId.Except(normalized).Any();

		if (isEqual) return Result.Success();

		_studentId.Clear();
		_studentId.AddRange(normalized);

		return Result.Success();
	}

	public bool HasStudent(Guid studentId) => _studentId.Contains(studentId);
	public IReadOnlyCollection<Guid> GetStudentIds() => _studentId.AsReadOnly();

	public Result UpdateName(string newName)
	{
		if (string.IsNullOrWhiteSpace(newName))
			return Result.Failure("Name is required");

		if (newName.Length > 300)
			return Result.Failure("Name length must be <= 300 characters");

		var trimmed = newName.Trim();

		if (trimmed == Name) 
			return Result.Success();

		Name = trimmed;
		return Result.Success();
	}		
}
