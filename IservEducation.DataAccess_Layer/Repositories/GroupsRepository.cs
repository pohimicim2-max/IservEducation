using IservEducation.Application_Layer.Interfaces;
using IservEducation.DataAccess_Layer.Entities;
using IservEducation.Domain_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace IservEducation.DataAccess_Layer.Repositories;

public class GroupsRepository : IGroupsRepository
{
	private readonly IservEducationDbContext _dbContext;

	public GroupsRepository(IservEducationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Guid> AddAsync(Group group)
	{
		if (group == null)
			throw new ArgumentNullException(nameof(group));

		var entityGroup = MapToEntity(group);

		await _dbContext.Groups.AddAsync(entityGroup);
		await _dbContext.SaveChangesAsync();

		return group.Id;
	}
	public async Task<Group?> GetByIdAsync(Guid id)
	{
		var findedGroup = await _dbContext.Groups
			.AsNoTracking()
			.Include(g => g.Students)
			.FirstOrDefaultAsync(g => g.Id == id);

		if (findedGroup == null)
			return null;

		return MapToDomain(findedGroup);
	}
	public async Task<Guid> DeleteByIdAsync(Guid id)
	{
		var findedGroup = await _dbContext.Groups.FindAsync(id);

		if (findedGroup == null)
			throw new ArgumentNullException(nameof(findedGroup));

		_dbContext.Groups.Remove(findedGroup);

		await _dbContext.SaveChangesAsync();
		return findedGroup.Id;
	}
	public async Task<Guid> UpdateAsync(Group group)
	{
		if (group == null)
			throw new ArgumentNullException(nameof(group));

		var existingEntity = await _dbContext.Groups
			.FirstOrDefaultAsync	(g => g.Id == group.Id);

		if (existingEntity == null)
			throw new ArgumentNullException($"Group with id {group.Id} not found");

		existingEntity.Name = group.Name;
		_dbContext.Groups.Update(existingEntity);
		await _dbContext.SaveChangesAsync();
		return existingEntity.Id;
	}

	public async Task<Group> AddStudentAsync(Guid groupId, Guid studentId)
	{
		var groupEntity = await _dbContext.Groups
			.Include(g => g.Students)
			.FirstOrDefaultAsync(g => g.Id == groupId);

		if (groupEntity == null)
			throw new ArgumentNullException(nameof(groupEntity));

		var studentEntity = await _dbContext.Students.FindAsync(studentId);

		if (studentEntity == null)
			throw new ArgumentNullException(nameof(studentEntity));

		if (!groupEntity.Students.Any(s => s.Id == studentId))
		{
			groupEntity.Students.Add(studentEntity);
			_dbContext.Groups.Update(groupEntity);
			await _dbContext.SaveChangesAsync();
		}

		return MapToDomain(groupEntity);
	}

	private GroupEntity MapToEntity(Group group)
	{
		return new GroupEntity
		{
			Id = group.Id,
			Name = group.Name
		};
	}
	private Group MapToDomain(GroupEntity entity)
	{
		var result = Group.Create(
			entity.Id,
			entity.Name
		);
		if (result.IsFailure)
			throw new InvalidOperationException($"Failed to map GroupEntity to Group: {result.Error}");

		var group = result.Value;

		if (entity.Students != null && entity.Students.Any())
		{
			var studentsId = entity.Students.Select(s => s.Id)
				.Where(id => id != Guid.Empty);

			group.ReplaceStudents(studentsId);
		}

		return result.Value;
	}
}
