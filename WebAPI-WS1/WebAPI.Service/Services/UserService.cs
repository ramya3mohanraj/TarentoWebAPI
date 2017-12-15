using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Service.Contract;

namespace WebAPI.Service.Services
{
    public class UserService : IUserService
    {
        public bool RegisterUser(Data.User user)
        {
            throw new NotImplementedException();
        }

        public List<Data.User> getAllUsers()
        {
            throw new NotImplementedException();
        }

        public Data.User Login(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
