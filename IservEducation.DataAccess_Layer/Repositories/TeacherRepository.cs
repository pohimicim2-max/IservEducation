using Microsoft.EntityFrameworkCore;
using IservEducation.DataAccess_Layer.Entities;
using IservEducation.Application_Layer.Interfaces;

namespace IservEducation.DataAccess_Layer.Repositories;

public class TeacherRepository : ITeacherRepository
{
	private readonly IservEducationDbContext _dbContext;

	public TeacherRepository(IservEducationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Guid> AddAsync(Teacher teacher)
	{
		if (teacher == null) 
			throw new ArgumentNullException(nameof(teacher));
		
		if (GetByLoginAsync(teacher.Login) != null)
			throw new InvalidOperationException($"User with login \"{teacher.Login}\" already exists");

		var entity = MapToEntity(teacher);
		
		await _dbContext.Teachers.AddAsync(entity);
		await _dbContext.SaveChangesAsync();

		return teacher.Id;
	}
	public async Task<Teacher?> GetByIdAsync(Guid id)
	{
		var findedTeacher = await _dbContext.Teachers
			.AsNoTracking()
			.FirstOrDefaultAsync(t => t.Id == id);

		if (findedTeacher == null)
			return null;

		return MapToDomain(findedTeacher);
	}
	public async Task<Guid> DeleteAsync(Guid id)
	{
		var findedTeacher = await _dbContext.Teachers.FindAsync(id);
		
		if (findedTeacher == null)
			throw new ArgumentNullException(nameof(findedTeacher));

		_dbContext.Teachers.Remove(findedTeacher);
		await _dbContext.SaveChangesAsync();

		return findedTeacher.Id;
	}
	public async Task<Guid> UpdateAsync(Teacher teacher)
	{
		if (teacher == null)
			throw new ArgumentNullException(nameof(teacher));

		if (GetByLoginAsync(teacher.Login) == null)
			throw new InvalidOperationException($"User with login \"{teacher.Login}\" is not found");

		var teacherEntity = MapToEntity(teacher);

		_dbContext.Teachers.Attach(teacherEntity);
		await _dbContext.SaveChangesAsync();

		return teacher.Id;
	}

	public async Task<Teacher?> GetByLoginAsync(string login)
	{
		if (string.IsNullOrWhiteSpace(login)) 
			return null;

		var findedTeacherEntity = await _dbContext.Teachers
			.AsNoTracking()
			.FirstOrDefaultAsync(t => t.Login == login);

		if (findedTeacherEntity == null)
			return null;

		return MapToDomain(findedTeacherEntity);
	}

	private static TeacherEntity MapToEntity(Teacher teacher)
	{
		var teacherEntity = new TeacherEntity
		{
			Id = teacher.Id,
			Login = teacher.Login,
			PasswordHash = teacher.PasswordHash,
			FirstName = teacher.FirstName,
			LastName = teacher.LastName,
			MiddleName = teacher.MiddleName
		};

		return teacherEntity;
	}
	private static Teacher MapToDomain(TeacherEntity entityTeacher)
	{
		var result = Teacher.Create(
			entityTeacher.Id,
			entityTeacher.Login,
			entityTeacher.PasswordHash,
			entityTeacher.FirstName,
			entityTeacher.LastName,
			entityTeacher.MiddleName
		);

		if (!result.IsSuccess)
			throw new InvalidOperationException($"Invalid data for TeacherEntity {entityTeacher.Id}: {result.Error}");

		var teacher = result.Value;

		if (entityTeacher.Lessons != null)
		{
			var lessonIds = entityTeacher.Lessons.Select(l => l.Id).Where(id => id != Guid.Empty);
			teacher.ReplaceLessons(lessonIds);
		}

		return teacher;
	}
}
