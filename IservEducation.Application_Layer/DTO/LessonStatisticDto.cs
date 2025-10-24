namespace IservEducation.Application_Layer.DTO;

public record LessonStatisticDTO (
	Guid Id, 
	Guid LessonId,
	Guid StudentId,
	bool Attendance,
	int CodeCoinCount 
);