namespace IservEducation.API_Layer.Conracts.Requests;

public record StudentRequest(
	string FirstName,
	string LastName,
	string? MiddleName,
	Guid? GroupID,
	int? CountCodeCoin
);
