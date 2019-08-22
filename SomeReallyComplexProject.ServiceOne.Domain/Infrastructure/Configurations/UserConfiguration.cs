using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate;

namespace SomeReallyComplexProject.ServiceOne.Domain.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                   .HasColumnName("UserId");

            builder.Property(o => o.Name)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Ignore(b => b.DomainEvents);
        }
    }
}
