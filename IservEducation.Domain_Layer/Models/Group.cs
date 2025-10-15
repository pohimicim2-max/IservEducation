using CSharpFunctionalExtensions;

namespace IservEducation.Domain_Layer.Models;

public sealed class Group
{
	public Guid Id { get; private set; }
	public string Name { get; private set; } = string.Empty;

	private readonly List<Guid> _studentIds = new();
	public IReadOnlyCollection<Guid> StudentIds => _studentIds.AsReadOnly();

	private Group() { }
	public static Result<Group> Create(Guid id, string name, IEnumerable<Guid>? initialStudentIds = null)
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

		if (initialStudentIds != null)
		{
			var normalized = initialStudentIds
				.Where(g => g != Guid.Empty)
				.Distinct()
				.ToList();

			group._studentIds.AddRange(normalized);
		}

		return Result.Success(group);
	}

	public Result AddStudent(Guid studentId)
	{
		if (studentId == Guid.Empty)
			return Result.Failure("StudentId must be a non-empty Guid");

		if (_studentIds.Contains(studentId))
			return Result.Failure("Student already in group");

		_studentIds.Add(studentId);
		return Result.Success();
	}

	public Result RemoveStudent(Guid studentId)
	{
		if (studentId == Guid.Empty)
			return Result.Failure("StudentId must be a non-empty Guid");

		var removed = _studentIds.Remove(studentId);

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

		bool isEqual = _studentIds.Count == normalized.Count && !_studentIds.Except(normalized).Any();

		if (isEqual) return Result.Success();

		_studentIds.Clear();
		_studentIds.AddRange(normalized);

		return Result.Success();
	}

	public bool HasStudent(Guid studentId) => _studentIds.Contains(studentId);
	public IReadOnlyCollection<Guid> GetStudentIds() => _studentIds.AsReadOnly();

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
