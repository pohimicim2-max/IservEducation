using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IservEducation.DataAccess_Layer.Entities;

namespace IservEducation.DataAccess_Layer.Configurations;

public class TeacherEntityConfiguration : IEntityTypeConfiguration<TeacherEntity>
{
	public void Configure(EntityTypeBuilder<TeacherEntity> builder)
	{
		builder.HasKey(t => t.Id);

		builder.Property(t => t.Login)
			   .IsRequired()
			   .HasMaxLength(100);

		builder.HasIndex(t => t.Login)
			   .IsUnique();

		builder.Property(t => t.PasswordHash)
			   .IsRequired();

		builder.Property(t => t.FirstName)
			   .IsRequired()
			   .HasMaxLength(150);

		builder.Property(t => t.LastName)
			   .IsRequired()
			   .HasMaxLength(150);

		builder.Property(t => t.MiddleName)
			   .HasMaxLength(150);

		// Teacher -> Lessons
		builder.HasMany(t => t.Lessons)
			   .WithOne(l => l.Teacher)
			   .HasForeignKey(l => l.TeacherId)
			   .OnDelete(DeleteBehavior.SetNull);
	}
}
