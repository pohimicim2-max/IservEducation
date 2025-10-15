namespace IservEducation.API_Layer.Requests;

public class CreateTeacherRequest
{
	public string Login { get; set; } = String.Empty;
	public string Password {  get; set; } = String.Empty;
	public string FirstName { get; set; } = String.Empty;
	public string LastName { get; set; } = String.Empty;
	public string? MiddleName { get; set; }
}
