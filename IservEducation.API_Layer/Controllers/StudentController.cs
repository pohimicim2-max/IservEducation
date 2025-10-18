using CSharpFunctionalExtensions;
using IservEducation.API_Layer.Conracts.Requests;
using IservEducation.API_Layer.Conracts.Responses;
using IservEducation.Application_Layer.Interfaces;
using IservEducation.Application_Layer.Services;
using Microsoft.AspNetCore.Mvc;

namespace IservEducation.API_Layer.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StudentController : ControllerBase
{
	private readonly IStudentsService _studentsService;

	public StudentController(IStudentsService studentsService)
	{
		_studentsService = studentsService;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] StudentRequest request)
	{
		if (request == null)
			return BadRequest("Request body is required.");

		var result = await _studentsService.CreateAsync(
			request.FirstName,
			request.LastName,
			request.MiddleName,
			request.GroupID,
			request.CountCodeCoin ?? 0
		);

		if(result.IsFailure)
			return BadRequest(result.Error);

		return Ok(new {UserId = result.Value});
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById([FromRoute] Guid id)
	{
		var result = await _studentsService.GetByIdAsync(id);

		if (result.IsFailure)
			return BadRequest(result.Error);

		var student = result.Value!;

		if (student == null)
			return NotFound();

		return Ok(new StudentResponse(
			student.FirstName,
			student.LastName,
			student.MiddleName,
			student.CountCodeCoin
		));
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeletedById([FromRoute] Guid id)
	{
		if (id == Guid.Empty)
			return BadRequest("Id is required");

		var result = await _studentsService.DeleteAsync(id);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok(new { UserId = result.Value });
	}
}

