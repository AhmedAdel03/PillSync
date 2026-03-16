using System;
using PillSync.Entites;

namespace PillSync.Data.Repo;

public interface IMemberRepo
{
    public Task<User>GetByEmail(string EmailAddress);
    public Task SaveChanges();
    public Task AddNewUser(User user);
    public Task<Member> GetMemberOTPs(string memberid);
        public Task PostOtp(OTP oTP);

 

}
