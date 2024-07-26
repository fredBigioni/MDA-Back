using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IUserPermissionService _userPermissionService;

        public UsersController(IUserService userService, IUserPermissionService userPermissionService)
        {
            _userService = userService;
            _userPermissionService = userPermissionService;
        }

        [AllowAnonymous]
        [HttpGet("ping")]
        public IActionResult Ping()
        {            
            return Ok("Ping");
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model, ipAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _userService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = _userService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserRequest user)
        {
            var _user = _userService.Create(user);
            return Ok(_user);
        } 

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserRequest user)
        {
            try {
                var result = _userService.Update(id, user);
                return Ok();
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/refresh-tokens")]
        public IActionResult GetRefreshTokens(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            return Ok(user.RefreshTokens);
        }

        [HttpGet("{id}/permissions")]
        public IActionResult GetPermissions(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            var permissions  = _userPermissionService.GetAllByUser(user);

            return Ok(permissions);
        }

        [HttpPut("{id}/permissions")]
        public IActionResult UpdatePermissions(int id, [FromBody] UserPermissionResponse userPermission)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            var permissions  = _userPermissionService.Update(user, userPermission);

            return Ok(permissions);
        }        

        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
