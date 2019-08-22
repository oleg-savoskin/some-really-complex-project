using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate;

namespace SomeReallyComplexProject.ServiceOne.Domain.Infrastructure.Configurations
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.ToTable("UserGroups");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                   .HasColumnName("UserGroupId");

            builder.Property(o => o.GroupName)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.HasOne<User>()
                   .WithMany("Groups")
                   .HasForeignKey(o => o.UserID);

            builder.Ignore(b => b.DomainEvents);
        }
    }
}
