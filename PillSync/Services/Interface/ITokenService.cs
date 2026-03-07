using System;
using PillSync.Entites;

namespace PillSync.Services.Interface;

public interface ITokenService
{
    public string CreateToken(User user);

}
