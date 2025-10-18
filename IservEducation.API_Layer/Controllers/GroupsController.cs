using IservEducation.API_Layer.Conracts.Requests;
using IservEducation.API_Layer.Conracts.Responses;
using IservEducation.Application_Layer.Interfaces;
using IservEducation.Application_Layer.Services;
using Microsoft.AspNetCore.Mvc;

namespace IservEducation.API_Layer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
	private readonly IGroupsService _groupsService;

	public GroupsController(IGroupsService groupsService)
	{
		_groupsService = groupsService;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateGroupRequest request)
	{
		if (request == null)
			return BadRequest("Request body is required.");

		var result = await _groupsService.CreateAsync(
			request.Name
		);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok(new { UserId = result.Value });
	}
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById([FromRoute] Guid id)
	{
		var result = await _groupsService.GetByIdAsync(id);

		if (result.IsFailure)
			return BadRequest(result.Error);

		var group = result.Value!;
		
		if (group == null)
			return NotFound();

		return Ok(group);
	}
	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteById([FromRoute] Guid id)
	{
		if (id == Guid.Empty)
			return BadRequest("Id is required");

		var result = await _groupsService.DeleteByIdAsync(id);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok(new { DeletedGroupId = result.Value });
	}

	[HttpPut("{id:guid}/students")]
	public async Task<IActionResult> AddStudent([FromRoute] Guid id, [FromQuery] Guid studentId)
	{
		if (id == Guid.Empty)
			return BadRequest("Group Id is required");

		if (studentId == Guid.Empty)
			return BadRequest("Student Id is required");

		var result = await _groupsService.AddStudentToGroupAsync(id, studentId);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok(new { UpdatedGroupId = result.Value });
	}

}
