using Microsoft.AspNetCore.Mvc;
using TaskAPI.Core.Entities;
using TaskAPI.Core.Interfaces;
using TaskAPI.Web.Attributes;

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
        [AuthorizeByRole("ADMIN","GUEST")]
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
        [AuthorizeByRole("ADMIN")]
        public async Task<IActionResult> UpdateUser(UserModel user) {

           var updatedUser = await _service.UpdateUser(user);

            if (updatedUser == null) {
                return NotFound("User id is wrong!");
            }

            return NoContent();
        }

        [HttpDelete("{userid}")]
        [AuthorizeByRole("ADMIN")]
        public async Task<IActionResult> DeleteUser(int userid) {

            var user = await _service.DeleteUser(userid);

            if (user == null) return NotFound("Invalid User Id!");

            return NoContent();

        }


    }
}
