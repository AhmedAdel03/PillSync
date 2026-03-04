using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PillSync.Entites;

namespace PillSync.Data.Config;

public class UsersConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
         builder.HasKey(x=>x.Id);
         builder.Property(x=>x.FullName).HasMaxLength(12);
         builder.HasOne(x=>x.Member).WithOne(x=>x.User).HasForeignKey<Member>(x=>x.Id);
    }
}
