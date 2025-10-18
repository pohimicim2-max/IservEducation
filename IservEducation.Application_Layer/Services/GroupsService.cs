using CSharpFunctionalExtensions;
using IservEducation.Application_Layer.DTO;
using IservEducation.Application_Layer.Interfaces;
using IservEducation.Domain_Layer.Models;

namespace IservEducation.Application_Layer.Services;

public class GroupsService : IGroupsService
{
	private readonly IGroupsRepository _groupRepository;
	private readonly IStudentsRepository _studentRepository;

	public GroupsService(IGroupsRepository groupsRepository, IStudentsRepository studentsRepository)
	{
		_groupRepository = groupsRepository;
		_studentRepository = studentsRepository;
	}

	public async Task<Result<Guid>> CreateAsync(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<Guid>("Name are required");

		var result = Group.Create(Guid.NewGuid(), name.Trim());

		if (!result.IsSuccess)
			return Result.Failure<Guid>(result.Error);

		var group = result.Value;

		await _groupRepository.AddAsync(group);
		return Result.Success(group.Id);
	}
	public async Task<Result<Guid>> DeleteByIdAsync(Guid id)
	{
		if (id == Guid.Empty)
			return Result.Failure<Guid>("id are required");

		var deletedId = await _groupRepository.DeleteByIdAsync(id);

		return Result.Success<Guid>(deletedId);
	}

	public async Task<Result<GroupDto>> GetByIdAsync(Guid id)
	{
		if (id == Guid.Empty)
			return Result.Failure<GroupDto>("id are required");

		var group = await _groupRepository.GetByIdAsync(id);

		if (group == null)
			return Result.Failure<GroupDto>($"Group with id {id} not found");

		var students = new List<StudentDTO>();

		foreach (var studentId in group.StudentIds)
		{
			var student = await _studentRepository.GetByIdAsync(studentId);
			if (student != null)
			{
				students.Add(new StudentDTO
				(
					student.Id,
					student.FirstName,
					student.LastName,
					student.MiddleName,
					student.GroupId,
					student.CountCodeCoin
				));
			}
		}

		var groupDto = new GroupDto(group.Id, group.Name, students);

		return Result.Success(groupDto);
	}

	public async Task<Result<Guid>> AddStudentToGroupAsync(Guid groupId, Guid studentId)
	{
		if (groupId == Guid.Empty)
			return Result.Failure<Guid>("groupId are required");

		if (studentId == Guid.Empty)
			return Result.Failure<Guid>("studentId are required");

		await _groupRepository.AddStudentAsync(groupId, studentId);

		return Result.Success(groupId);
	}
}
