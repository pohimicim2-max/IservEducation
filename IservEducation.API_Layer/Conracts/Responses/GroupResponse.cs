namespace IservEducation.API_Layer.Conracts.Responses;

public record GroupResponse(
	string Name,
	List <StudentResponse> Students
);
