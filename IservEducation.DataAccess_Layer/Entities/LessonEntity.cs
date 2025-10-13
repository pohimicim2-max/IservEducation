namespace IservEducation.DataAccess_Layer.Entities;

public class LessonEntity
{
	public Guid Id { get; set; }

	// Дата урока
	public DateTime Date { get; set; }

	// Переменная для определения отработка ли это
	public bool IsGapfill { get; set; } = false;

	// Привязка к преподавателю
	public Guid? TeacherId { get; set; }					
	public TeacherEntity? Teacher { get; set; } 

	// Привязка к группе учеников
	public Guid? GroupId { get; set; }
	public GroupEntity? Group { get; set; }

	// Список статистики по каждому ученику
	public virtual ICollection<LessonStatisticEntity> LessonStatistics { get; set; } = new List<LessonStatisticEntity>();
}
