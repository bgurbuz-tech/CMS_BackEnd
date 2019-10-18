using Microsoft.AspNetCore.Mvc;
using CMS_App.Interfaces;
using CMS_App.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.Extensions.Configuration;

namespace CMS_App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly IUserRep _userRep;
        private readonly IConfiguration config;

        public UsersController(IUserRep userRep, IConfiguration configuration)
        {
            _userRep = userRep;
            config = configuration;
        }

        [HttpGet("getall")]
        public IEnumerable<User> GetAll()
        {
            return _userRep.GetAllUsers();
        }

        // GET api/users/1
        //[HttpGet("{id}")]
        //public async Task<User> Get(string id)
        //{
        //    return await _userRep.GetUser(id) ?? new User();
        //}

        [Authorize]
        [HttpPost("useraccount")]
        public IActionResult UserAccount([FromBody]User userParam)
        {
            User user = null;
            user = _userRep.GetUser(userParam.UserEmail);
            if (user != null)
            {
                user.UserPassword = "";
                return Ok(user);
            }
            else
            {
                return BadRequest(new { message = "Kullanıcı da sıkıntı var!" });
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var kod = _userRep.Authenticate(userParam.UserEmail, userParam.UserPassword);
            if (!kod)
                return BadRequest(new { message = "E-posta adresi veya şifre yanlış!" });
            else
            {
                var token = GenerateToken(userParam.UserEmail);
                return Ok(token);
            }

        }

        private string GenerateToken(string userName)
        {
            var someClaims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Email,userName),
                new Claim(JwtRegisteredClaimNames.NameId,Guid.NewGuid().ToString())
            };

            SecurityKey securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Secret").Value));
            var token = new JwtSecurityToken(
                issuer: "gurbuz.betul",
                audience: "gurbuz.betul@gmail.com",
                claims: someClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            var kod = _userRep.RemoveUser(id);
            if (!kod)
                return BadRequest(new { message = "Kullanıcı silinemedi!Delete" });
            else
            {
                return Ok(new { message = "Kullanıcı silindi!" });
            }
        }

        [Authorize]
        [HttpPost("adduser")]
        public IActionResult AddUser([FromBody]User userParam)
        {
            var kod = _userRep.AddUser(userParam);
            if (!kod)
                return BadRequest(new { message = "Kullanıcı kaydedilemedi.Insert" });
            else
            {
                return Ok(new { message = "Kullanıcı kaydedildi!" });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id,[FromBody]User userParam)
        {
            var kod = _userRep.UpdateUser(id,userParam);
            if (!kod)
                return BadRequest(new { message = "Kullanıcı güncellenemedi.Update" });
            else
            {
                return Ok(new { message = "Kullanıcı güncellendi!" });
            }
        }
    }
}