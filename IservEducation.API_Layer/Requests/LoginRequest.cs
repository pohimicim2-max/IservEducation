namespace IservEducation.API_Layer.Requests;

public record LoginRequest (
	string Login,
	string Password
);