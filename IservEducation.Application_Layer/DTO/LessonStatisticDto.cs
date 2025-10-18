namespace IservEducation.Application_Layer.DTO;

public record LessonStatisticDto (
	Guid Id, 
	Guid LessonId,
	Guid StudentId,
	bool Attendance,
	int CodeCoinCount 
);