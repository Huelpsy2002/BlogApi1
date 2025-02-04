using BlogApi.BussinssLogic;
using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersLogic _userLogic;

        public UsersController(IUsersLogic userLogic)
        {
            _userLogic = userLogic;
        }


        [HttpPost("add")]
        public async Task<ActionResult>AddUser(AddUserDto addUserDto)
        {
            try
            {
                var(success,errors) =  await _userLogic.AddUser(addUserDto);

                if (!success)
                {
                    return BadRequest(errors);

                }
                return Ok(new { message = "User Created" });

            }


            catch (Exception err)
            {
                throw new Exception($"{err.Message}");
            }
           
        }


        [HttpPatch("{username}")]
        public async Task<ActionResult>UpdateUser(string username,updateUserDto updateuserdto)
        {
            try
            {
                var (success, errors) = await _userLogic.UpdateUser(username,updateuserdto);

                if (!success)
                {
                    return BadRequest(errors);

                }
                return Ok(new { message = "User updated" });

            }


            catch (Exception err)
            {
                throw new Exception($"{err.Message}");
            }
        }


        [HttpGet("{username}")]
        public async Task<ActionResult<getUserDetailsDto>>GetUser(string username)
        {
            try
            {
                var user = await _userLogic.getUser(username);
                if ( user== null)
                {
                    return NotFound(new { message = $"user with username {username} does not exist" });
                }
                return Ok(user);

            }
            catch(Exception err)
            {
                throw new Exception($"{err.Message}");

            }
        }


        [HttpDelete("{username}")]
        public async Task<ActionResult>DeleteUser(string username)
        {
            try
            {
                var deleted = await _userLogic.deleteUser(username);
                if (!deleted)
                {
                    return NotFound(new { message = $"user with username {username} does not exist" });
                }
                return Ok(new {message = "user deleted."});

            }
            catch (Exception err)
            {
                throw new Exception($"{err.Message}");

            }
        }
    }
}
