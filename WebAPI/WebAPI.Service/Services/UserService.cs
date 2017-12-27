using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Service.Contract;
using WebAPI.Service.Repository;

namespace WebAPI.Service.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AuthRepository repo = new AuthRepository();

        public UserService()
        {
        }

        public async Task<IdentityResult> RegisterUser(User user)
        {
            //using (ApplicationDbContext dbContext = new ApplicationDbContext())
            //{
            //    dbContext.Users.Add(user);
            //    db.SaveChanges();
            //    return true;
            //}
            IdentityResult result =  await repo.RegisterUser(user);
            //if(response != null)
            //    result = response.Result;
            return result;
        }

        public List<User> getAllUsers()
        {
            //List<Users> users = db.Users.ToList();
            List<User> result = repo.GetAllUsers();
            return result;
        }

        public async Task<User> Login(string userName, string password)
        {
            //return db.Users.Where(u => u.UserName == userName && u.Password == password).FirstOrDefault();
            User result = await repo.FindUser(userName, password);
            return result;
        }
    }
}
