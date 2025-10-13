using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IservEducation.DataAccess_Layer.Entities;

namespace IservEducation.DataAccess_Layer.Configurations;

public class StudentEntityConfiguration : IEntityTypeConfiguration<StudentEntity>
{
	public void Configure(EntityTypeBuilder<StudentEntity> builder)
	{
		builder.HasKey(s => s.Id);

		builder.Property(s => s.FirstName)
			   .IsRequired()
			   .HasMaxLength(150);

		builder.Property(s => s.LastName)
			   .IsRequired()
			   .HasMaxLength(150);

		builder.Property(s => s.MiddleName)
			   .HasMaxLength(150);

		builder.Property(s => s.CodeCoinCount)
			   .IsRequired();

		// Student -> LessonStatistics
		builder.HasMany(s => s.LessonStatistics)
			   .WithOne(ls => ls.Student)
			   .HasForeignKey(ls => ls.StudentId)
			   .OnDelete(DeleteBehavior.Cascade);
	}
}
