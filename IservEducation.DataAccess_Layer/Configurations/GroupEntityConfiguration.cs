using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IservEducation.DataAccess_Layer.Entities;

namespace IservEducation.DataAccess_Layer.Configurations;

public class GroupEntityConfiguration : IEntityTypeConfiguration<GroupEntity>
{
	public void Configure(EntityTypeBuilder<GroupEntity> builder)
	{
		builder.HasKey(g => g.Id);

		builder.Property(g => g.Name)
			   .IsRequired()
			   .HasMaxLength(300);

		// Group -> Students
		builder.HasMany(g => g.Students)
			   .WithOne(s => s.Group)
			   .HasForeignKey(s => s.GroupId)
			   .OnDelete(DeleteBehavior.SetNull);

		// Group -> Lessons
		builder.HasMany(g => g.Lessons)
			   .WithOne(l => l.Group)
			   .HasForeignKey(l => l.GroupId)
			   .OnDelete(DeleteBehavior.SetNull);
	}
}
