using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(t => t.Date).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(255);
        builder.Property(t => t.UserId).IsRequired();

        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.PaymentMethod)
            .WithMany()
            .HasForeignKey(t => t.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
