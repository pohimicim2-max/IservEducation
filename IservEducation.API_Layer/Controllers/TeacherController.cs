namespace IservEducation.API_Layer.Controllers;

using IservEducation.API_Layer.Conracts.Requests;
using IservEducation.API_Layer.Conracts.Responses;
using IservEducation.Application_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
	private readonly ITeacherService _teacherService;

	public TeacherController(ITeacherService teacherService)
	{
		_teacherService = teacherService;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateTeacherRequest request)
	{
		if (request == null) 
			return BadRequest("Request body is required.");

		var result = await _teacherService.CreateAsync(
			request.Login,
			request.Password,
			request.FirstName,
			request.LastName,
			request.MiddleName
		);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok(new { UserId = result.Value });
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginRequest request)
	{
		if (request == null) 
			return BadRequest("Request body is required.");

		var result = await _teacherService.LoginAsync(request.Login, request.Password);

		if (result.IsFailure)
			return Unauthorized(result.Error);

		var teacher = result.Value!;

		var response = new AuthResponse(
			teacher.Id,
			teacher.Login,
			teacher.FirstName,
			teacher.LastName,
			teacher.MiddleName);

		return Ok(response);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById([FromRoute] Guid id)
	{
		var result = await _teacherService.GetByIdAsync(id);

		if(result.IsFailure)
			return BadRequest(result.Error);

		var teacher = result.Value!;

		if (teacher == null)
			return NotFound();

		return Ok(new
		{
			teacher.Id,
			teacher.Login,
			teacher.FirstName,
			teacher.LastName,
			teacher.MiddleName,
			Lessons = teacher.LessonIds
		});
	}
}
