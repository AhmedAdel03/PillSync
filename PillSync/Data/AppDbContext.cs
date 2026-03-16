using System;
using Microsoft.EntityFrameworkCore;
using PillSync.Entites;

namespace PillSync.Data;

public class AppDbContext(DbContextOptions options):DbContext(options)
{
public DbSet<User> Users { get; set; }
public DbSet<Member> Members { get; set; }
public DbSet<Photo> Photos { get; set; }
public DbSet<OTP> OTPCode { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
