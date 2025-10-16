using CSharpFunctionalExtensions;

namespace IservEducation.Application_Layer.Interfaces;

public interface ITeachersService
{
	Task<Result<Guid>> CreateAsync(string login, string password, string firstName, string lastName, string? middleName = null);
	Task<Result<Teacher>> LoginAsync(string login, string password);
	Task<Result<Teacher>> GetByIdAsync(Guid id);
}