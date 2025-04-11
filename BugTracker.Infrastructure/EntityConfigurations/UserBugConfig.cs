using BugTracker.Data.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BugTracker.Infrastructure.EntityConfigurations
{
    public class UserBugConfig : IEntityTypeConfiguration<UserBugs>
    {
        public void Configure(EntityTypeBuilder<UserBugs> builder)
        {
            builder.HasKey(x => new { x.BugId, x.UserId });
            builder.HasOne(x => x.User).WithMany(x => x.userBugs).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Bug).WithMany(x => x.userBugs).HasForeignKey(x => x.BugId);
        }
    }
}
