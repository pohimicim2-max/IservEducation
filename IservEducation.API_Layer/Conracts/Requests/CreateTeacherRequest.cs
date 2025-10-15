namespace IservEducation.API_Layer.Conracts.Requests;

public record CreateTeacherRequest(
	string Login,
	string Password,
	string FirstName,
	string LastName,
	string? MiddleName
);
