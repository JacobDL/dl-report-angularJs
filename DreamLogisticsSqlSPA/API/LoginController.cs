using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamLogisticsSqlSPA.ControllerLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repository.Models;
using Repository.Repositories;
using static DreamLogisticsSqlSPA.ControllerLogic.UserServiceLogic;

namespace DreamLogisticsSqlSPA.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //private IUserService _userService;

        //public LoginController(IUserService userService)
        //{
        //    _userService = userService;
        //}

        [HttpPost]
        public IActionResult GetUser(AuthenticateModel user)
        {
            User validUser = UserRepository.ValidateUser(user);
            //UserServiceLogic service = new UserServiceLogic();
            //validUser = service.Authenticate(validUser.Username, validUser.Password);
            return Ok(validUser);
        }

        //[AllowAnonymous]
        //[HttpPost("authenticate")]
        //public IActionResult Authenticate([FromBody]AuthenticateModel model)
        //{
        //    var user = _userService.Authenticate(model.Username, model.Password);

        //    if (user == null)
        //        return BadRequest(new { message = "Username or password is incorrect" });

        //    return Ok(user);
        //}

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var users = _userService.GetAll();
        //    return Ok(users);
        //}

    }
}