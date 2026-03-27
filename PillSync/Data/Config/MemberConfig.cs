using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PillSync.Entites;

namespace PillSync.Data.Config;

public class MemberConfig : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasMany(x=>x.Photos).WithOne(x=>x.Member).HasForeignKey(x=>x.MemberId);
        builder.HasMany(x=>x.OTPs).WithOne(x=>x.member).HasForeignKey(x=>x.MemberId);
        builder.HasMany(x=>x.Medicines).WithOne(x=>x.member).HasForeignKey(x=>x.MemberId);
    }
}
