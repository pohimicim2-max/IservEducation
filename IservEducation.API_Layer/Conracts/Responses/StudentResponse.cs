namespace IservEducation.API_Layer.Conracts.Responses;

public record StudentResponse(
	string FirstName,
	string LastName,
	string? MiddleName,
	Guid? GroupID,
	int? CountCodeCoin
);
