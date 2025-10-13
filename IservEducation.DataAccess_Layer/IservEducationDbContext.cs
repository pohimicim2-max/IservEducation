using IservEducation.DataAccess_Layer.Configurations;
using IservEducation.DataAccess_Layer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.EducationService.Infrastructure.Persistence
{
	public class IservEducationDbContext : DbContext
	{
		public IservEducationDbContext(DbContextOptions<IservEducationDbContext> options)
			: base(options)
		{
		}
		public DbSet<TeacherEntity> Teachers => Set<TeacherEntity>();
		public DbSet<GroupEntity> Groups => Set<GroupEntity>();
		public DbSet<StudentEntity> Students => Set<StudentEntity>();
		public DbSet<LessonEntity> Lessons => Set<LessonEntity>();
		public DbSet<LessonStatisticEntity> LessonStatistics => Set<LessonStatisticEntity>();

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
}
