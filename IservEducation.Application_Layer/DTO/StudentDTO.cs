using IservEducation.Domain_Layer.Models;

namespace IservEducation.Application_Layer.DTO;

public record StudentDTO(
	Guid Id,
	string FirstName,
	string LastName,
	string? MiddleName,
	Guid? GroupID,
	int? CountCodeCoin
);