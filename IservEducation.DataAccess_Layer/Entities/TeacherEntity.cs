namespace IservEducation.DataAccess_Layer.Entities;

public class TeacherEntity
{
	public Guid Id { get; set; } 

	public string Login { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;

	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string MiddleName { get; set; } = string.Empty;

	// Список проставленных уроков
	public virtual ICollection<LessonEntity> Lessons { get; set; } = new List<LessonEntity>();
}
	