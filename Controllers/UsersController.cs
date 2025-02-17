using BlogApi.BussinssLogic;
using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Security.Claims;

namespace BlogApi.Controllers
{
    [Route("BlogApi")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersLogic _userLogic;

        public UsersController(IUsersLogic userLogic)
        {
            _userLogic = userLogic;
        }




        [HttpPost("users/login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<string>>Login(LoginDto loginDto)
        {

            try
            {
               
                if (loginDto == null)
                {
                    return BadRequest(new { message = "Invalid Data." });
                }
                var (auth, token) = await _userLogic.AuthenticateAsync(loginDto);

                if (!auth)
                {
                    return Unauthorized(new { message = "invalid username/Email or password" });
                }
                else
                {
                    return Ok(new { token = token });
                }
            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.StackTrace });

            }



        }






        [HttpPost("users/register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult>Register(AddUserDto addUserDto)
        {
            try
            {
           
                var (success,errors) =  await _userLogic.AddUser(addUserDto);

                if (!success)
                {
                    return BadRequest(new { errors=errors });

                }
                return Ok(new { message = "User Created" });

            }


            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });
            }

        }




        [Authorize]
        [HttpPatch("users/user/update")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult>UpdateUser(updateUserDto updateuserdto)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
               
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }
                var (success, errors) = await _userLogic.UpdateUser(username,updateuserdto);

                if (!success)
                {
                    return BadRequest(new { errors });

                }
                return Ok(new { message = "User updated" });

            }


            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });
            }
        }

        [Authorize]

        [HttpGet("users/user")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<getUserDetailsDto>>GetUser()
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }
                var user = await _userLogic.getUser(username);
                if ( user== null)
                {
                    return NotFound(new { message = $"user with username {username} does not exist" });
                }
                return Ok(new { user });

            }
            catch(Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });

            }
        }

        [Authorize]

        [HttpDelete("users/user/delete")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult>DeleteUser()
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { error = "Invalid or missing authentication token." });
                }

                var deleted = await _userLogic.deleteUser(username);
                if (!deleted)
                {
                    return NotFound(new { message = $"user with username {username} does not exist" });
                }
                return Ok(new {message = "user deleted."});

            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = "An unexpected error occurred", details = err.Message });

            }
        }


    }
}
