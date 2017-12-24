using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Service.Contract;

namespace WebAPI.Service.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public UserService()
        {
        }

        public bool RegisterUser(User user)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                dbContext.User.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public List<User> getAllUsers()
        {
            List<User> users = db.User.ToList();
            return users;
        }

        public User Login(string userName, string password)
        {
            return db.User.Where(u => u.UserName == userName && u.Password == password).FirstOrDefault();
        }
    }
}
