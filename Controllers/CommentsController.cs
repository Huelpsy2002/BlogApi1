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
    public class CommentsController : ControllerBase
    {


        private readonly ICommentsLogic _CommentLogic;
        public CommentsController(ICommentsLogic commentsLogic)
        {
            _CommentLogic = commentsLogic;
        }


        [Authorize]
        [HttpPost("comments/{blogId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Comments(int blogId)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }

                return Ok(_CommentLogic.GetAllComments(blogId));

            }


            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });
            }

        }


        [Authorize]
        [HttpPost("comments/{blogId}/addComment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AddComment(AddCommentsDto addCommentsDto,int blogId)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }
                var (success, errors) = await _CommentLogic.AddComment(addCommentsDto, username,blogId);

                if (!success)
                {
                    return BadRequest(new { errors });

                }
                return Ok(new { message = "comment Created" });

            }


            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });
            }

        }



        [Authorize]
        [HttpPatch("comments/{commentId}/updateComment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> updateComment(updateCommentsDto updateCommentdto,int commentId)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }
                var (success, errors) = await _CommentLogic.updateComment(updateCommentdto, username, commentId);

                if (!success)
                {
                    return BadRequest(new { errors });

                }
                return Ok(new { message = "comment updated" });

            }


            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });
            }
        }

        [Authorize]

        [HttpGet("comments/{blogId}/getComment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<getCommentsDto>> GetComment(int blogId)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }
                var blog = await _CommentLogic.getComment(username, blogId);
                if (blog == null)
                {
                    return NotFound(new { message = $"comment  does not exist" });
                }
                return Ok(new { blog });

            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });

            }
        }

        [Authorize]
        [HttpDelete("comments/{commentId}/deleteComment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }

                var deleted = await _CommentLogic.deleteComment(username, commentId);
                if (!deleted)
                {
                    return NotFound(new { message = $"comment does not exist" });
                }
                return Ok(new { message = "comment deleted." });

            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });

            }
        }

    }
}

