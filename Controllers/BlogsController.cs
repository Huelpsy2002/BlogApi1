using BlogApi.BussinssLogic;
using BlogApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApi.Controllers
{
    [Route("BlogApi")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogsLogic _blogLogic;
        public BlogsController(IBlogsLogic blogLogic)
        {
            _blogLogic = blogLogic;
        }


        [Authorize]
        [HttpPost("Blogs")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Blogs()
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }
               
                return Ok(_blogLogic.GetAllBlogs(username));

            }


            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });
            }

        }


        [Authorize]
        [HttpPost("Blogs/Add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AddBlog(AddBlogDto addblogdto)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }
                var (success, errors) = await _blogLogic.AddBlog(addblogdto,username);

                if (!success)
                {
                    return BadRequest(new { errors });

                }
                return Ok(new { message = "blog Created" });

            }


            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });
            }

        }



        [Authorize]
        [HttpPatch("blogs/update")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> updateBlog(updateBlogDto updateBlogdto)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }
                var (success, errors) = await _blogLogic.UpdateBlog(updateBlogdto, username);

                if (!success)
                {
                    return BadRequest(new { errors });

                }
                return Ok(new { message = "blog updated" });

            }


            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });
            }
        }

        [Authorize]

        [HttpGet("blogs/getBlog/{blogId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<getUserDetailsDto>> GetBlog(int blogId)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }
                var blog = await _blogLogic.getBlog(username,blogId);
                if (blog == null)
                {
                    return NotFound(new { message = $"blog with id {blogId} does not exist" });
                }
                return Ok(new { blog });

            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });

            }
        }

        [Authorize]
        [HttpDelete("blogs/delete/{blogId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteBlog(int blogId)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }

                var deleted = await _blogLogic.deleteBlog(username,blogId);
                if (!deleted)
                {
                    return NotFound(new { message = $"blog with id {blogId } does not exist" });
                }
                return Ok(new { message = "blog deleted." });

            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });

            }
        }

    }
}
