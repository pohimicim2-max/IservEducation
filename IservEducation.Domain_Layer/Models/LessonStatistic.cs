using CSharpFunctionalExtensions;

namespace IservEducation.Domain_Layer.Models;

public sealed class LessonStatistic
{
	public Guid Id { get; private set; }
	public Guid LessonId { get; private set; }
	public Guid StudentId { get; private set; }

	public bool Attendance { get; private set; }
	public int CodeCoinCount { get; private set; }

	private LessonStatistic() { }

	public static Result<LessonStatistic> Create(
		Guid id,
		Guid lessonId,
		Guid studentId,
		bool attendance = false,
		int codeCoinCount = 0)
	{
		if (id == Guid.Empty)
			return Result.Failure<LessonStatistic>("Id must be a non-empty Guid");

		if (lessonId == Guid.Empty)
			return Result.Failure<LessonStatistic>("LessonId must be a non-empty Guid");

		if (studentId == Guid.Empty)
			return Result.Failure<LessonStatistic>("StudentId must be a non-empty Guid");

		if (codeCoinCount < 0)
			return Result.Failure<LessonStatistic>("CodeCoinCount must be >= 0");

		var stat = new LessonStatistic
		{
			Id = id,
			LessonId = lessonId,
			StudentId = studentId,
			Attendance = attendance,
			CodeCoinCount = codeCoinCount
		};

		return Result.Success(stat);
	}

	public Result MarkAttendance(bool attended, int codeCoinsEarned = 0)
	{
		if (codeCoinsEarned < 0)
			return Result.Failure("codeCoinsEarned must be >= 0");

		Attendance = attended;
		CodeCoinCount = codeCoinsEarned;
		return Result.Success();
	}

	public Result SetCodeCoinCount(int codeCoins)
	{
		if (codeCoins < 0)
			return Result.Failure("codeCoins must be >= 0");

		CodeCoinCount = codeCoins;
		return Result.Success();
	}

	public void MarkAbsentAndResetCoins()
	{
		Attendance = false;
		CodeCoinCount = 0;
	}
}