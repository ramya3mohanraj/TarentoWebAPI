using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Common.Logger;
using WebAPI.Data;
using WebAPI.Service.Services;

namespace WebAPI.Controllers
{
    
    public class UserController : ApiController
    {
        private UserService _userService = new UserService();

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAllUsers()
        {
            List<User> response = _userService.getAllUsers();
            return Ok(response);
        }

        [HttpPost]
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

            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Login(string userName, string password)
        {
            Token token = new Token();
            string baseAddress = "http://localhost:61654/";
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", userName},
                   {"password", password},
               };
                var tokenResponse = client.PostAsync(baseAddress + "/token", new FormUrlEncodedContent(form)).Result;
                //var token = tokenResponse.Content.ReadAsStringAsync().Result;  
                token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                //User response = await _userService.Login(userName, password);

            }
            return Ok(token);
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
