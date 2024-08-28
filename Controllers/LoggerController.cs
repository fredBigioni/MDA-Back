using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoggerController : ControllerBase
    {
        private ILoggerService _loggerService;
        private readonly DataContext _context;

        public LoggerController(ILoggerService loggerService, DataContext context)
        {
            _loggerService = loggerService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Log>>> GetAllLogs()
        {
            try
            {
                var logs = await _loggerService.GetAllLogs(_context);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
