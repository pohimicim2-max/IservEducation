namespace IservEducation.DataAccess_Layer.Entities;

public class LessonStatisticEntity
{
	public Guid Id { get; set; } 

	// Идентификатор урока
	public Guid LessonId { get; set; } = Guid.Empty;
	public LessonEntity? Lesson { get; set; } 

	// Идентификатор ученика
	public Guid StudentId { get; set; } = Guid.Empty;
	public StudentEntity? Student { get; set; }

	// Хранит был ли учник на уроке
	public bool Attendance { get; set; } = false;
	// Количество заработанных КодКоинов за урок
	public int CodeCoinCount { get; set; } = 0;
}