using System;
using Microsoft.EntityFrameworkCore;
using PillSync.Entites;

namespace PillSync.Data.Repo;

public class MemberRepo(AppDbContext context) : IMemberRepo
{
    public async Task AddNewUser(User user)
    {
        await context.Users.AddAsync(user);
    }
      public async Task<User> GetByID(string UserId)
    {
        var user= await context.Users.Include(x=>x.Member).FirstOrDefaultAsync(x=>x.Id==UserId);
        return user;
    }

    public async Task<User> GetByEmail(string EmailAddress)
    {
        var user= await context.Users.Include(x=>x.Member).FirstOrDefaultAsync(x=>x.EmailAddress==EmailAddress);
        return user;
    }

    public async Task<Member> GetMemberOTPs(string memberid)
    {
        var member=await context.Members.Include(x=>x.OTPs.Where(x=>x.IsRevoked==false)).FirstOrDefaultAsync(x=>x.Id==memberid);
        return member;
    }

    public async Task PostOtp(OTP oTP)
    {
         await context.AddAsync(oTP);
    }

    public async Task SaveChanges()
    {
        await context.SaveChangesAsync();
    }
}
