using ISUCorp.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISUCorp.Infra.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .UseIdentityColumn();

            builder.Property(e => e.AddedAt)
                .IsRequired();

            builder.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("null");

            builder.Property(e => e.Date)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(e => e.Notes)
                .HasDefaultValueSql("null")
                .HasColumnType("ntext");

            builder.Property(e => e.Rating)
                .HasDefaultValueSql("null");

            builder.Property(e => e.IsFavorite)
                .HasDefaultValueSql("0");

            builder.HasOne<Contact>(e => e.Contact)
                .WithMany(c => c.Reservations)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne<Place>(e => e.Place)
                .WithMany(c => c.Reservations)
                .HasForeignKey(e => e.PlaceId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
