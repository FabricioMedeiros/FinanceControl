using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(255);
        builder.Property(c => c.Type).IsRequired();
        builder.Property(c => c.UserId).IsRequired();

        builder.HasIndex(c => new { c.UserId, c.Name }).IsUnique();
    }
}
