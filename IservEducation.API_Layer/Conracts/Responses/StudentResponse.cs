using IservEducation.Domain_Layer.Models;

namespace IservEducation.API_Layer.Conracts.Responses;

public record StudentResponse(
	string FirstName,
	string LastName,
	string? MiddleName,
	int? CountCodeCoin
);
