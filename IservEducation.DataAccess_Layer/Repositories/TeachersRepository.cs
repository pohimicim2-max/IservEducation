using Microsoft.EntityFrameworkCore;
using IservEducation.DataAccess_Layer.Entities;
using IservEducation.Application_Layer.Interfaces;

namespace IservEducation.DataAccess_Layer.Repositories;

public class TeachersRepository : ITeachersRepository
{
	private readonly IservEducationDbContext _dbContext;

	public TeachersRepository(IservEducationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Guid> AddAsync(Teacher teacher)
	{
		if (teacher == null)
			throw new ArgumentNullException(nameof(teacher));

		var existingTeacher = await GetByLoginAsync(teacher.Login);

		if (existingTeacher != null)
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

		var existingEntity = await _dbContext.Teachers
			.FirstOrDefaultAsync(t => t.Id == teacher.Id);

		if (existingEntity == null)
			throw new InvalidOperationException($"Teacher with id {teacher.Id} not found");

		var loginExists = await _dbContext.Teachers
			.AnyAsync(t => t.Login == teacher.Login && t.Id != teacher.Id);

		if (loginExists)
			throw new InvalidOperationException($"User with login \"{teacher.Login}\" already exists");

		existingEntity.Login = teacher.Login;
		existingEntity.PasswordHash = teacher.PasswordHash;
		existingEntity.FirstName = teacher.FirstName;
		existingEntity.LastName = teacher.LastName;
		existingEntity.MiddleName = teacher.MiddleName;

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
	private static Teacher MapToDomain(TeacherEntity teacherEntity)
	{
		var result = Teacher.Create(
			teacherEntity.Id,
			teacherEntity.Login,
			teacherEntity.PasswordHash,
			teacherEntity.FirstName,
			teacherEntity.LastName,
			teacherEntity.MiddleName
		);

		if (!result.IsSuccess)
			throw new InvalidOperationException($"Invalid data for TeacherEntity {teacherEntity.Id}: {result.Error}");

		var teacher = result.Value;

		if (teacherEntity.Lessons != null)
		{
			var lessonIds = teacherEntity.Lessons
				.Select(l => l.Id)
				.Where(id => id != Guid.Empty);

			teacher.ReplaceLessons(lessonIds);
		}

		return teacher;
	}
}
