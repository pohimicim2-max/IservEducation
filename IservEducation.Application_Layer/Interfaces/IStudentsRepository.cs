using IservEducation.Domain_Layer.Models;

namespace IservEducation.Application_Layer.Interfaces;

public interface IStudentsRepository
{
	Task<Guid> AddAsync(Student student);
	Task<Guid> DeleteAsync(Guid id);
	Task<Student?> GetByIdAsync(Guid id);
	Task<Guid> UpdateAsync(Student student);
}