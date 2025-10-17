using CSharpFunctionalExtensions;
using IservEducation.Domain_Layer.Models;

namespace IservEducation.Application_Layer.Interfaces
{
	public interface IStudentsService
	{
		Task<Result<Guid>> CreateAsync(string firstName, string lastName, string? middleName = null, Guid? groupId = null, int countCodeCoin = 0);
		Task<Result<Student>> GetByIdAsync(Guid id);
		Task<Result<Guid>> DeleteAsync(Guid id);
	}
}