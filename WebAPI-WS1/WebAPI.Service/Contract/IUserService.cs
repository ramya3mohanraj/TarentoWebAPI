using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Service.Contract
{
    public interface IUserService
    {
        bool RegisterUser(User user);
        List<User> getAllUsers();
        User Login(string userName, string password);
    }
}
