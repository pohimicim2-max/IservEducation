using CSharpFunctionalExtensions;
using IservEducation.Application_Layer.DTO;
using IservEducation.Domain_Layer.Models;

namespace IservEducation.Application_Layer.Interfaces;

public interface IGroupsService
{
	Task<Result<Guid>> CreateAsync(string name);
	Task<Result<Guid>> DeleteByIdAsync(Guid id);
	Task<Result<GroupDto>> GetByIdAsync(Guid id);
	Task<Result<Guid>> AddStudentToGroupAsync(Guid groupId, Guid studentId);
}