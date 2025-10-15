using BCrypt.Net;
using IservEducation.Application_Layer.Interfaces;

public class BCryptPasswordHasher : IPasswordHasher
{
	public string Hash(string password)
	{
		if (string.IsNullOrWhiteSpace(password))
			throw new ArgumentException("Password cannot be empty", nameof(password));

		return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
	}

	public bool Verify(string hash, string password)
	{
		if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(password))
			return false;

		return BCrypt.Net.BCrypt.Verify(password, hash);
	}
}
