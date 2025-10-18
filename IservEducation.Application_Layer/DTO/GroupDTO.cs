namespace IservEducation.Application_Layer.DTO;

public record GroupDto(
	Guid Id,
	string Name,
	List<StudentDTO> Students
);
