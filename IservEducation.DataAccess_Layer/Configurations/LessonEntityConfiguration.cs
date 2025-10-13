using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IservEducation.DataAccess_Layer.Entities;

namespace IservEducation.DataAccess_Layer.Configurations;

public class LessonEntityConfiguration : IEntityTypeConfiguration<LessonEntity>
{
	public void Configure(EntityTypeBuilder<LessonEntity> builder)
	{
		builder.HasKey(l => l.Id);

		builder.Property(l => l.Date)
			   .IsRequired();

		builder.Property(l => l.IsGapfill)
			   .IsRequired();

		// Lesson -> LessonStatistics
		builder.HasMany(l => l.LessonStatistics)
			   .WithOne(ls => ls.Lesson)
			   .HasForeignKey(ls => ls.LessonId)
			   .OnDelete(DeleteBehavior.Cascade);

		// Lesson -> Teacher
		builder.HasOne(l => l.Teacher)
			   .WithMany(t => t.Lessons)
			   .HasForeignKey(l => l.TeacherId)
			   .OnDelete(DeleteBehavior.SetNull);

		// Lesson -> Group
		builder.HasOne(l => l.Group)
			   .WithMany(g => g.Lessons)
			   .HasForeignKey(l => l.GroupId)
			   .OnDelete(DeleteBehavior.SetNull);
	}
}
