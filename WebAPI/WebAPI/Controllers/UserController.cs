using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Data;
using WebAPI.Service.Services;

namespace WebAPI.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetRegisteredUsers()
        {
            UserService _userService = new UserService();
            List<User> response = _userService.getAllUsers();
            return Ok(response);
        }

    }
}
