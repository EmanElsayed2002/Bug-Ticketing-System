using BugTracker.Application.Services.Abstract.Auth;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.User;

namespace BugTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _role;

        public RoleController(IRoleService role)
        {
            _role = role;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {

            var res = await _role.GetAllAsync();
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }

        [HttpPost("add-Role")]
        public async Task<IActionResult> AddRole(RoleRequest role)
        {

            var res = await _role.CreateAsync(role);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }
        [HttpPut("update-Role")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleRequest role)
        {

            var res = await _role.UpdateAsync(id, role);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }
        [HttpDelete("delete-Role")]
        public async Task<IActionResult> DeleteRole(string id)
        {

            var res = await _role.DeleteAsync(id);
            return res.Match(res => Ok(res), error => Problem(statusCode: error.statusCode, title: error.Code, detail: error.Descriptin));

        }

    }
}
