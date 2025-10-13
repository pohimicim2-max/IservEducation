namespace IservEducation.DataAccess_Layer.Entities;

public class GroupEntity
{
	public Guid Id { get; set; }
	// Название группы. Пример: "Новочебоксарск-Никольский. Воскресенье. Junior - 10:00"
	public string Name { get; set; } = string.Empty;
	// Список учеников в группе
	public virtual ICollection<StudentEntity>? Students { get; set; } = new List<StudentEntity>();
	// Список уроков у группы
	public virtual ICollection<LessonEntity>? Lessons { get; set; } = new List<LessonEntity>();
}