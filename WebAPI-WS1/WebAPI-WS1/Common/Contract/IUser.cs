using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.Contract
{
    public interface IUser
    {
        bool Register(User user);
        List<User> getAllUsers();
        User Login(string userName, string password);
    }
}
