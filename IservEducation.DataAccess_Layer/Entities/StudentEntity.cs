namespace IservEducation.DataAccess_Layer.Entities;

public class StudentEntity
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string MiddleName { get; set; } = string.Empty;
																					
	// Список сатистики по урокам
	public virtual ICollection<LessonStatisticEntity> LessonStatistics { get; set; } = new List<LessonStatisticEntity>();

	// Идентификатор группы
	public Guid? GroupId { get; set; }
	public GroupEntity? Group { get; set; }

	// Суммарное количество кодкоинов у ученика
	public int CodeCoinCount { get; set; } = 0;
}