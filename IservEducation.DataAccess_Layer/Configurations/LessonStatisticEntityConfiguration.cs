using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IservEducation.DataAccess_Layer.Entities;

namespace IservEducation.DataAccess_Layer.Configurations;

public class LessonStatisticEntityConfiguration : IEntityTypeConfiguration<LessonStatisticEntity>
{
	public void Configure(EntityTypeBuilder<LessonStatisticEntity> builder)
	{
		builder.HasKey(ls => ls.Id);

		builder.Property(ls => ls.Attendance)
			   .IsRequired();

		builder.Property(ls => ls.CodeCoinCount)
			   .IsRequired();

		// LessonStatistic -> Lesson
		builder.HasOne(ls => ls.Lesson)
			   .WithMany(l => l.LessonStatistics)
			   .HasForeignKey(ls => ls.LessonId)
			   .OnDelete(DeleteBehavior.Cascade);

		// LessonStatistic -> Student
		builder.HasOne(ls => ls.Student)
			   .WithMany(s => s.LessonStatistics)
			   .HasForeignKey(ls => ls.StudentId)
			   .OnDelete(DeleteBehavior.Cascade);

		// один студент — одна статистика на один урок
		builder.HasIndex(ls => new { ls.LessonId, ls.StudentId })
			   .IsUnique();
	}
}
