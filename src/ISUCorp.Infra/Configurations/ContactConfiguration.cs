using ISUCorp.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISUCorp.Infra.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .UseIdentityColumn();

            builder.HasIndex(e => e.Name).IsUnique();

            builder.Property(e => e.AddedAt)
                .IsRequired();

            builder.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("null");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasColumnType("tinyint");

            builder.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasDefaultValueSql("null");

            builder.Property(e => e.BirthDate)
                .IsRequired();
        }
    }
}
