using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_App.Interfaces;
using CMS_App.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMS_App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PostsController : Controller
    {
        private readonly IPostRep _iPostRep;
        private readonly IConfiguration config;

        public PostsController(IPostRep iPostRep, IConfiguration configuration)
        {
            _iPostRep = iPostRep;
            config = configuration;
        }

        [AllowAnonymous]
        [HttpGet("getposts")]
        public IEnumerable<Post> GetPosts()
        {
            return _iPostRep.GetPosts();
        }

        [Authorize]
        [HttpPost("addpost")]
        public IActionResult AddPost([FromBody]Post item)
        {
            var kod = _iPostRep.AddPost(item);
            if (!kod)
                return BadRequest(new { message = "Post kaydedilemedi.Insert" });
            else
            {
                return Ok(new { message = "Post kaydedildi!" });
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdatePost(string id, [FromBody]Post item)
        {
            var kod = _iPostRep.UpdatePost(id, item);
            if (!kod)
                return BadRequest(new { message = "Post güncellenemedi.Update" });
            else
            {
                return Ok(new { message = "Post güncellendi!" });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetPost(string id)
        {
            Post post = _iPostRep.GetPost(id);
            if (post != null)
            {
                return Ok(post);
            }
            else
            {
                return BadRequest(new { message = "Post getirilemedi!"});
            }
        }

        [Authorize]
        [HttpPost("{id}")]
        public IActionResult ArchievePost(string id)
        {
            var kod = _iPostRep.ArchievePost(id);
            if (!kod)
                return BadRequest(new { message = "Post arşivlenemedi.Archieve" });
            else
            {
                return Ok(new { message = "Post arşivlendi!" });
            }
        }

    }
}
