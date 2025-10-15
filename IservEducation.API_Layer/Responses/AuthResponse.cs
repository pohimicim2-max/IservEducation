namespace IservEducation.API_Layer.Responses;

public record AuthResponse(
	Guid TeacherId,
	string Login,
	string FirstName,
	string LastName,
	string? MiddleName
);
