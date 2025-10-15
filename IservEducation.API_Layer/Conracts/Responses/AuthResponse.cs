namespace IservEducation.API_Layer.Conracts.Responses;

public record AuthResponse(
	Guid TeacherId,
	string Login,
	string FirstName,
	string LastName,
	string? MiddleName
);
