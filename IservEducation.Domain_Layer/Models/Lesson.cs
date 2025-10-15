using CSharpFunctionalExtensions;
using IservEducation.Domain_Layer.Models;

public sealed class Lesson
{
	public Guid Id { get; private set; }
	public Guid TeacherId { get; private set; }
	public Guid GroupId { get; private set; }

	public DateTime Date { get; private set; }
	public bool IsGapfill { get; private set; }

	/// <summary>Список статистики учеников по этому уроку.</summary>
	private readonly List<LessonStatistic> _lessonStatistics = new();
	public IReadOnlyCollection<LessonStatistic> LessonStatistics => _lessonStatistics.AsReadOnly();

	private Lesson() { }

	public static Result<Lesson> Create(
		Guid id,
		Guid teacherId,
		Guid groupId,
		DateTime date,
		bool isGapfill = false)
	{
		if (id == Guid.Empty)
			return Result.Failure<Lesson>("Id must be non-empty Guid.");

		if (teacherId == Guid.Empty)
			return Result.Failure<Lesson>("TeacherId must be non-empty Guid.");

		if (groupId == Guid.Empty)
			return Result.Failure<Lesson>("GroupId must be non-empty Guid.");

		if (date == default)
			return Result.Failure<Lesson>("Date must be valid.");

		var lesson = new Lesson
		{
			Id = id,
			TeacherId = teacherId,
			GroupId = groupId,
			Date = date,
			IsGapfill = isGapfill
		};

		return Result.Success(lesson);
	}

	// -----------------------------
	// Методы работы с LessonStatistics
	// -----------------------------

	public Result AddLessonStatistic(LessonStatistic statistic)
	{
		if (statistic == null)
			return Result.Failure("LessonStatistic cannot be null.");

		if (statistic.LessonId != Id)
			return Result.Failure("LessonStatistic.LessonId must match this lesson Id.");

		if (_lessonStatistics.Any(s => s.StudentId == statistic.StudentId))
			return Result.Failure("Statistic for this student already exists.");

		_lessonStatistics.Add(statistic);
		return Result.Success();
	}

	public Result RemoveLessonStatistic(Guid statisticId)
	{
		var stat = _lessonStatistics.FirstOrDefault(s => s.Id == statisticId);
		if (stat is null)
			return Result.Failure("Statistic not found.");

		_lessonStatistics.Remove(stat);
		return Result.Success();
	}
	public bool HasStatisticForStudent(Guid studentId) =>
		_lessonStatistics.Any(s => s.StudentId == studentId);

	public LessonStatistic? GetStatisticForStudent(Guid studentId) =>
		_lessonStatistics.FirstOrDefault(s => s.StudentId == studentId);

	public Result MarkStudentAttendance(Guid studentId, bool attended, int codeCoins)
	{
		var stat = _lessonStatistics.FirstOrDefault(s => s.StudentId == studentId);
		if (stat is null)
			return Result.Failure("Statistic for student not found.");

		return stat.MarkAttendance(attended, codeCoins);
	}
}