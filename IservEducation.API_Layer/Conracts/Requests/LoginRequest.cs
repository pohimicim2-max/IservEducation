namespace IservEducation.API_Layer.Conracts.Requests;

public record LoginRequest (
	string Login,
	string Password
);