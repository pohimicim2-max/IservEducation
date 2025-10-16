using IservEducation.Application_Layer.Interfaces;
using IservEducation.DataAccess_Layer.Entities;
using IservEducation.Domain_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace IservEducation.DataAccess_Layer.Repositories;

public class StudentsRepository : IStudentsRepository
{
	private readonly IservEducationDbContext _dbContext;
	public StudentsRepository(IservEducationDbContext dbContext)
	{
		_dbContext = dbContext;
	}


	public async Task<Guid> AddAsync(Student student)
	{
		if (student == null)
			throw new ArgumentNullException(nameof(student));

		var entityStudent = MapToEntity(student);

		await _dbContext.Students.AddAsync(entityStudent);
		await _dbContext.SaveChangesAsync();

		return student.Id;
	}
	public async Task<Student?> GetByIdAsync(Guid id)
	{
		var findedStudent = await _dbContext.Students
			.AsNoTracking()
			.FirstOrDefaultAsync(s => s.Id == id);

		if (findedStudent == null)
			return null;

		return MapToDomain(findedStudent);
	}
	public async Task<Guid> DeleteAsync(Guid id)
	{
		var findedStudent = await _dbContext.Students.FindAsync(id);

		if (findedStudent == null)
			throw new ArgumentNullException(nameof(findedStudent));

		_dbContext.Students.Remove(findedStudent);
		await _dbContext.SaveChangesAsync();

		return findedStudent.Id;
	}
	public async Task<Guid> UpdateAsync(Student student)
	{
		if (student == null)
			throw new ArgumentNullException(nameof(student));

		var existingEntity = await _dbContext.Students
			.FirstOrDefaultAsync(s => s.Id == student.Id);

		if (existingEntity == null)
			throw new ArgumentNullException($"Student with id {student.Id} not found");

		existingEntity.FirstName = student.FirstName;
		existingEntity.LastName = student.LastName;
		existingEntity.MiddleName = student.MiddleName;
		existingEntity.CodeCoinCount = student.CountCodeCoin;

		await _dbContext.SaveChangesAsync();
		return existingEntity.Id;
	}

	private StudentEntity MapToEntity(Student student)
	{
		var entityStudent = new StudentEntity
		{
			Id = student.Id,
			FirstName = student.FirstName,
			LastName = student.LastName,
			MiddleName = student.MiddleName,
			GroupId = student.GroupId,
			CodeCoinCount = student.CountCodeCoin
		};
		return entityStudent;
	}
	private Student MapToDomain(StudentEntity studentEntity)
	{
		var result = Student.Create(
			studentEntity.Id,
			studentEntity.FirstName,
			studentEntity.LastName,
			studentEntity.MiddleName,
			studentEntity.GroupId,
			studentEntity.CodeCoinCount
		);

		if (result.IsFailure)
			throw new InvalidOperationException($"Invalid data for StudentEntity {studentEntity.Id}: {result.Error}");

		var student = result.Value;

		if (studentEntity.LessonStatistics != null)
		{
			var lessonsStatisticId = studentEntity.LessonStatistics
				.Select(ls => ls.Id)
				.Where(id => id != Guid.Empty);

			student.ReplaceLessonsStatistic(lessonsStatisticId);
		}
		return student;
	}
}
