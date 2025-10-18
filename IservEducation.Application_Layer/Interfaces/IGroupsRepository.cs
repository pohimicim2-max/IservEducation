using IservEducation.Domain_Layer.Models;

namespace IservEducation.Application_Layer.Interfaces;

public interface IGroupsRepository
{
	Task<Guid> AddAsync(Group group);
	Task<Guid> DeleteByIdAsync(Guid id);
	Task<Group?> GetByIdAsync(Guid id);
	Task<Guid> UpdateAsync(Group group);
	Task<Group> AddStudentAsync(Guid groupId, Guid studentId);
}