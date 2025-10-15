namespace IservEducation.Application_Layer.Interfaces;

public interface IPasswordHasher
{
	string Hash(string password);
	bool Verify(string hash, string password);
}
