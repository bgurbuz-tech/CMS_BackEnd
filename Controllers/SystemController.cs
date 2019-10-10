using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_App.Interfaces;
using CMS_App.Model;
using Microsoft.AspNetCore.Mvc;

namespace CMS_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IUserRep _userRep;

        public SystemController(IUserRep userRep)
        {
            _userRep = userRep;
        }

        // Call an initialization - api/system/init
        [HttpGet("{setting}")]
        public string Get(string setting)
        {
            if (setting == "init")
            {
                try
                {
                    _userRep.AddUser(new User()
                    {
                        UserEmail = "abc@g.com",
                        UserPassword = "111",
                        UserStatus = 1,
                        UserType = 1,
                        UserCreated = DateTime.Now,
                        UserModified = DateTime.Now
                    });

                    _userRep.AddUser(new User()
                    {
                        UserEmail = "abc1@g.com",
                        UserPassword = "111",
                        UserStatus = 1,
                        UserType = 1,
                        UserCreated = DateTime.Now,
                        UserModified = DateTime.Now
                    });

                    _userRep.AddUser(new User()
                    {
                        UserEmail = "admin@g",
                        UserPassword = "111",
                        UserStatus = 1,
                        UserType = 1,
                        UserCreated = DateTime.Now,
                        UserModified = DateTime.Now
                    });

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

                return "Database CMS_Db was created, and collection 'Users' was filled with 2 sample items";
            }

            return "Unknown";
        }
    }
}