using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Data;
using WebAPI.Service.Services;

namespace WebAPI.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private UserService _userService = new UserService();

        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            List<IdentityUser> response = _userService.getAllUsers();
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RegisterUser(User userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _userService.RegisterUser(userModel);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IHttpActionResult> Login(string userName, string password)
        {
            IdentityUser response = await _userService.Login(userName, password);
            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
