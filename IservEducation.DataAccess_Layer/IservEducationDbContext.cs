using IservEducation.DataAccess_Layer.Configurations;
using IservEducation.DataAccess_Layer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IservEducation.DataAccess_Layer;

public class IservEducationDbContext : DbContext
{
	private readonly IConfiguration _configuration;

	public IservEducationDbContext(IConfiguration confuguration)
	{
		_configuration = confuguration;
	}
	public DbSet<TeacherEntity> Teachers => Set<TeacherEntity>();
	public DbSet<GroupEntity> Groups => Set<GroupEntity>();
	public DbSet<StudentEntity> Students => Set<StudentEntity>();
	public DbSet<LessonEntity> Lessons => Set<LessonEntity>();
	public DbSet<LessonStatisticEntity> LessonStatistics => Set<LessonStatisticEntity>();
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			var connectionString = _configuration.GetConnectionString("DefaultConnection");
			optionsBuilder.UseNpgsql(connectionString);
		}
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new TeacherEntityConfiguration());
		modelBuilder.ApplyConfiguration(new GroupEntityConfiguration());
		modelBuilder.ApplyConfiguration(new StudentEntityConfiguration());
		modelBuilder.ApplyConfiguration(new LessonEntityConfiguration());
		modelBuilder.ApplyConfiguration(new LessonStatisticEntityConfiguration());
	}
}
