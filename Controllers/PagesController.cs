using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_App.Interfaces;
using CMS_App.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : Controller
    {
        private readonly IPage _pageRep;
        private readonly IConfiguration config;

        public IActionResult Index()
        {
            return View();
        }

        public PagesController(IPage pageRep, IConfiguration configuration)
        {
            _pageRep = pageRep;
            config = configuration;
        }

        [AllowAnonymous]
        [HttpGet("GetIndex")]
        public Page GetIndex()
        {
            return _pageRep.GetIndex();
        }

        [AllowAnonymous]
        [HttpGet("GetAboutUs")]
        public Page GetAboutUs()
        {
            return _pageRep.GetAboutUs();
        }

        [AllowAnonymous]
        [HttpGet("GetContact")]
        public Page GetContact()
        {
            return _pageRep.GetContact();
        }

        [Authorize]
        [HttpGet("GetAllPages")]
        public IEnumerable<Page> GetAllPages()
        {
            return _pageRep.GetAllPages();
        }

        [Authorize]
        [HttpPost("UpdatePage")]
        public IActionResult UpdatePage([FromBody]Page page)
        {
            var kod = _pageRep.UpdatePage(page);
            if (kod)
                return Ok(new { message = "Sayfa güncellendi." });
            else
                return BadRequest(new { message = "Hata oluştu sayfa güncellenemedi! NoChange" });

        }
    }
}