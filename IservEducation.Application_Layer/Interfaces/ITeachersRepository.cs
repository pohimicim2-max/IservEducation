namespace IservEducation.Application_Layer.Interfaces;

public interface ITeachersRepository
{
	Task<Guid> AddAsync(Teacher teacher);
	Task<Guid> DeleteAsync(Guid id);
	Task<Teacher?> GetByIdAsync(Guid id);
	Task<Teacher?> GetByLoginAsync(string login);
	Task<Guid> UpdateAsync(Teacher teacher);
}
