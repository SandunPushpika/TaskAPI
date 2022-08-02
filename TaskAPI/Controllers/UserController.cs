using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Core.Entities;
using TaskAPI.Core.Interfaces;

namespace TaskAPI.Web.Controllers {

    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly IUserService _service;

        public UserController(IUserService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers() {

            var res = await _service.GetAllUsers();
            return Ok(res);

        }

        [HttpGet("{username}")]
        [Authorize(Roles = "ADMIN,GUEST")]
        public async Task<IActionResult> GetUserByUsername(string username) {

            var user = await _service.GetUserByUsername(username);
            return Ok(user);

        }

        [HttpPost("register")]
        public async Task<IActionResult> AddNewUser([FromBody] UserModel user) {

            var res = await _service.AddNewUser(user);

            if (res == null) {

                return Conflict("User Already Exists!");
            }

            return Created($"/{res.Username}", res);

        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateUser(UserModel user) {

            await _service.UpdateUser(user);

            return NoContent();
        }

        [HttpDelete("/{username}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUser(string username) {

            await _service.DeleteUser(username);
            return NoContent();

        }


    }
}
