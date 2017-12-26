using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Service.Contract
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUser(User user);
        List<IdentityUser> getAllUsers();
        Task<IdentityUser> Login(string userName, string password);
    }
}
