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
            //    dbContext.User.Add(user);
            //    db.SaveChanges();
            //    return true;
            //}
            IdentityResult result =  await repo.RegisterUser(user);
            //if(response != null)
            //    result = response.Result;
            return result;
        }

        public List<IdentityUser> getAllUsers()
        {
            //List<User> users = db.User.ToList();
            List<IdentityUser> result = repo.GetAllUsers();
            return result;
        }

        public async Task<IdentityUser> Login(string userName, string password)
        {
            //return db.User.Where(u => u.UserName == userName && u.Password == password).FirstOrDefault();
            IdentityUser result = await repo.FindUser(userName, password);
            return result;
        }
    }
}
